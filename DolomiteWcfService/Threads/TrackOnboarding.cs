﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using DolomiteModel;
using TagLib;

namespace DolomiteWcfService.Threads
{
    class TrackOnboarding
    {

        #region Properties and Constants

        private const int SleepSeconds = 10;

        private static DatabaseManager DatabaseManager { get; set; }

        private static LocalStorageManager LocalStorageManager { get; set; }

        private static TrackManager TrackManager { get; set; }

        #endregion

        #region Start/Stop Logic

        private volatile bool _shouldStop = false;

        /// <summary>
        /// Sets the stop flag on the thread loop
        /// </summary>
        public void Stop()
        {
            _shouldStop = true;
        }

        #endregion

        public void Run()
        {
            // Set up the thread with some managers
            DatabaseManager = DatabaseManager.Instance;
            LocalStorageManager = LocalStorageManager.Instance;
            TrackManager = TrackManager.Instance;

            // Loop until the stop flag has been flown
            while (!_shouldStop)
            {                
                // Try to get a work item
                Guid? workItemId = DatabaseManager.GetOnboardingWorkItem();
                if (workItemId.HasValue)
                {
                    // We have work to do!
                    Trace.TraceInformation("Work item {0} picked up by {1}", workItemId.Value.ToString(), GetHashCode());

                    // Calculate the hash and look for a duplicate
                    try
                    {
                        string hash = CalculateHash(workItemId.Value);
                    }
                    catch (DuplicateNameException)
                    {
                        // There was a duplicate. Delete it from storage and delete the initial record
                        Trace.TraceError("{1} determined track {0} was a duplicate. Removing record...", workItemId, GetHashCode());
                        DatabaseManager.DeleteTrack(workItemId.Value);
                        LocalStorageManager.DeleteFile(workItemId.Value.ToString());
                        continue;
                    }
                    
                    // The file was not a duplicate, so continue processing it
                    // Grab the metadata for the track
                    try
                    {
                        StoreMetadata(workItemId.Value);
                    }
                    catch (UnsupportedFormatException)
                    {
                        // Failed to determine type. We don't want this file.
                        Trace.TraceError("{1} failed to determine the type of track {0}. Removing record...", workItemId, GetHashCode());
                        DatabaseManager.DeleteTrack(workItemId.Value);
                        LocalStorageManager.DeleteFile(workItemId.Value.ToString());
                        continue;
                    }

                    CreateQualities(workItemId.Value);

                }
                else
                {
                    // No Work items. Sleep.
                    Trace.TraceInformation("No work items for {0}. Sleeping...", GetHashCode());
                    Thread.Sleep(TimeSpan.FromSeconds(SleepSeconds));
                }
            }
        }

        #region Onboarding Methods

        /// <summary>
        /// Calculates the RIPEMD160 hash of the track with the given guid and
        /// stores it to the database.
        /// </summary>
        /// <param name="trackGuid">The track to calculate the hash for</param>
        /// <returns>The hah of the file</returns>
        private string CalculateHash(Guid trackGuid)
        {
            // Grab an instance of the track
            try
            {
                Trace.TraceInformation("{0} is calculating hash for {1}", GetHashCode(), trackGuid);
                FileStream track = LocalStorageManager.RetrieveFile(trackGuid.ToString());

                // Calculate the hash and save it
                RIPEMD160 hashCalculator = RIPEMD160Managed.Create();
                byte[] hashBytes = hashCalculator.ComputeHash(track);
                string hashString = BitConverter.ToString(hashBytes);
                hashString = hashString.Replace("-", String.Empty);

                // CLOSE THE STREAM!
                track.Close();

                // Is the track a duplicate?
                if (DatabaseManager.GetTrackByHash(hashString) != null)
                {
                    // The track is a duplicate!
                    throw new DuplicateNameException(String.Format("Track {0} is a duplicate as determined by hash comparison", trackGuid));
                }

                // Store that hash to the database
                DatabaseManager.SetTrackHash(trackGuid, hashString);

                return hashString;
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception from {0} while calculating hash of {1}: {2}", GetHashCode(), trackGuid, e.Message);
                throw;
            }
        }

        private void CreateQualities(Guid trackGuid)
        {
            // Grab the track that will be manipulated
            var track = DatabaseManager.GetTrackModelByGuid(trackGuid);

            // Fetch all supported qualities
            var qualitites = DatabaseManager.GetAllQualities();
            int originalQuality = track.OriginalBitrate.Value;

            // Figure out what qualities to use by picking all qualities with bitrates
            // lessthan or equal to the original (+/- 5kbps -- for lousy sources)
            // ReSharper disable PossibleInvalidOperationException  (this isn't possible since the fetching method does not select nulls)
            var maxQuality = qualitites.Where(q => Math.Abs(q.Bitrate.Value - originalQuality) <= 5);
            var lessQualities = qualitites.Where(q => q.Bitrate < originalQuality);
            var requiredQualities = lessQualities.Union(maxQuality);
            // ReSharper restore PossibleInvalidOperationException
            
            // Generate new audio files for each quality that is required
            foreach (Quality quality in requiredQualities)
            {
                string inputFilename = LocalStorageManager.GetPath(trackGuid.ToString());
                string outputFilename =
                    LocalStorageManager.GetPath(String.Format("{0}.{1}.{2}", trackGuid, quality.Bitrate.Value,
                                                              quality.Extension));
                string arguments = String.Format("-i \"{2}\" -acodec {0} -ab {1}000 -y \"{3}\"", quality.Codec,
                                                 quality.Bitrate.Value, inputFilename, outputFilename);

                // Borrowing some code from http://stackoverflow.com/a/8726175
                ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = @"Externals\ffmpeg.exe",
                        Arguments = arguments,
                        CreateNoWindow = true,
                        ErrorDialog = false,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        RedirectStandardError = true
                    };

                // Launch the process
                Trace.TraceInformation("Launching {0} {1}", psi.FileName, psi.Arguments);
                using (Process exeProcess = Process.Start(psi))
                {
                    string outString = string.Empty;
                    
                    // use ansynchronous reading for at least one of the streams
                    // to avoid deadlock
                    exeProcess.OutputDataReceived += (s, e) =>
                        {
                            outString += e.Data;
                        };
                    exeProcess.BeginOutputReadLine();
                    
                    // now read the StandardError stream to the end
                    // this will cause our main thread to wait for the
                    // stream to close (which is when ffmpeg quits)
                    string errString = exeProcess.StandardError.ReadToEnd();
                    Trace.TraceWarning(errString);
                     
                }
            }
        }

        /// <summary>
        /// Strips the metadata from the track and stores it to the database
        /// Also retrieves the mimetype in the process.
        /// </summary>
        /// <param name="trackGuid">The guid of the track to store metadata of</param>
        private void StoreMetadata(Guid trackGuid)
        {
            Trace.TraceInformation("{0} is retrieving metadata from {1}", GetHashCode(), trackGuid);

            // Generate the mimetype of the track
            // Why? b/c tag lib isn't smart enough to figure it out for me,
            // except for determining it based on extension -- which is silly.
            FileStream localFile = LocalStorageManager.RetrieveFile(trackGuid.ToString());
            string mimetype = MimetypeDetector.GetMimeType(localFile);
            if (mimetype == null)
            {
                localFile.Close();
                throw new UnsupportedFormatException(String.Format("The mimetype of {0} could not be determined from the file header.", trackGuid));
            }

            // Retrieve the file from temporary storage
            TagLib.File file = TagLib.File.Create(LocalStorageManager.GetPath(trackGuid.ToString()), mimetype, ReadStyle.Average);
            
            Dictionary<int, string> metadata = new Dictionary<int, string>();

            Dictionary<string, int> acceptedTags = DatabaseManager.GetAllowedMetadataFields();
            
            // Use reflection to iterate over the properties in the tag
            PropertyInfo[] properties = typeof (Tag).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
                object value = property.GetValue(file.Tag);

                // Strip off "First" from the tag names
                name = name.Replace("First", string.Empty);

                // Skip tags that aren't the list of acceptable tags
                if (!acceptedTags.ContainsKey(name))
                    continue;

                // We really only want strings to store and ints
                if(value is string)
                    metadata.Add(acceptedTags[name], (string)value);
                else if(value is uint && (uint)value != 0)
                    metadata.Add(acceptedTags[name], ((uint)value).ToString());
            }

            // Send the metadata to the database
            DatabaseManager.StoreTrackMetadata(trackGuid, metadata);

            // Store the audio metadata to the database
            DatabaseManager.StoreAudioQualityInfo(trackGuid, file.Properties.AudioBitrate,
                                                  file.Properties.AudioSampleRate, file.MimeType);
        }

        #endregion

    }
}
