﻿using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DolomiteModel;

namespace DolomiteManagement
{
    public class LocalStorageManager
    {
        /// <summary>
        /// The path to the local resources folder
        /// </summary>
        public static string LocalResourcePath { get; set; }

        #region Singleton Instance

        private static LocalStorageManager _instance;

        /// <summary>
        /// Singleton instance of the Azure Storage manager
        /// </summary>
        public static LocalStorageManager Instance
        {
            get { return _instance ?? (_instance = new LocalStorageManager()); }
        }

        /// <summary>
        /// Singleton constructor for the AzureStorageManager
        /// </summary>
        private LocalStorageManager() {}

        #endregion

        /// <summary>
        /// Generates the path to the file in local storage
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the local storage path was not set before using
        /// </exception>
        /// <param name="filename">The name of the file</param>
        /// <returns>String path to the file</returns>
        public string GetPath(string filename)
        {
            // Make sure that we have a local path before loading it
            if(String.IsNullOrWhiteSpace(LocalResourcePath))
                throw new InvalidOperationException("Local storage path has not been initialized.");

            // Build the path of the file
            return Path.Combine(LocalResourcePath, filename);
        }

        #region Storage Methods

        /// <summary>
        /// Stores the contents of the stream to the file with the given file name
        /// <remarks>
        /// This automatically uploads to the onboarding storage. It may be
        /// necessary to expand this to support other local storage later.
        /// </remarks>
        /// </summary>
        /// <param name="stream">The stream to store</param>
        /// <param name="filename">The name of the file</param>
        /// <param name="owner">The owner of the file</param>
        [Obsolete]
        public string StoreStream(Stream stream, string filename, string owner)
        {
            // Copy the stream to the file
            using (var newFile = File.Create(GetPath(filename)))
            {
                stream.CopyTo(newFile);
            }

            // Calculate the hash of the file
            return CalculateHash(stream, owner);
        }

        /// <summary>
        /// Stores the contents of the stream to the file with the given filename.
        /// </summary>
        /// <param name="stream">The stream to store</param>
        /// <param name="filename">
        /// The filename to store the file to (usually the GUID of the track)
        /// </param>
        public async Task StoreStreamAsync(Stream stream, string filename)
        {
            // Copy the stream to the local temporary storage
            using (FileStream newFile = File.Create(GetPath(filename)))
            {
                await stream.CopyToAsync(newFile);
            }
        }

        #endregion

        #region Retrieval Methods

        /// <summary>
        /// Retrieves the given filename from the onboarding storage 
        /// <remarks>
        /// YOU MUST BE SURE TO CLOSE THE FILE STREAM AFTER USE!!!
        /// </remarks>
        /// </summary>
        /// <param name="filename">The name of the file to retrieve</param>
        /// <returns>The filestream of the file requested</returns>
        public FileStream RetrieveReadableFile(string filename)
        {
            // Return a stream of the file
            return File.OpenRead(GetPath(filename));
        }

        #endregion

        #region Deletion Methods

        /// <summary>
        /// Deletes the file with the fiven name from the onboarding storage
        /// </summary>
        /// <param name="filename">Name of the file to delete</param>
        public void DeleteFile(string filename)
        {
            Trace.TraceInformation("Attempting to delete {0} from local storage, w/o wait", filename);

            // Delete the file
            string filePath = GetPath(filename);
            if (File.Exists(filePath))
            {
                File.Delete(GetPath(filename));
            }
        }

        #endregion

        #region Hashing Methods

        /// <summary>
        /// Calculates the RIPEMD160 hash of the given stream
        /// </summary>
        /// <param name="stream">The stream to calculate the hash of</param>
        /// <param name="owner">The owner of the track</param>
        /// <returns>The hash of the file</returns>
        /// TODO: Don't check for track stuff in here! Or, create a separate method for calculating art hashes and determining if they are different
        [Obsolete("Use CalculateMd5Hash, check existing hash in your own logic")]
        public string CalculateHash(Stream stream, string owner)
        {
            stream.Position = 0;

            // Calculate the hash and save it
            RIPEMD160 hashCalculator = RIPEMD160.Create();
            byte[] hashBytes = hashCalculator.ComputeHash(stream);
            string hashString = BitConverter.ToString(hashBytes);
            hashString = hashString.Replace("-", String.Empty);

            stream.Position = 0;

            // Is the track a duplicate?
            if (TrackDbManager.Instance.GetTrackByHash(hashString, owner) != null)
            {
                // The track is a duplicate!
                throw new DuplicateNameException("Track is a duplicate as determined by hash comparison");
            }

            return hashString;
        }
        
        /// <summary>
        /// Calculates an MD5 hash of the file specified by <paramref name="filename"/>
        /// </summary>
        /// <param name="filename">The file to hash</param>
        /// <returns>The MD5 hash of the file</returns>
        [Pure]
        public string CalculateMd5Hash(string filename)
        {
            using (FileStream file = File.OpenRead(GetPath(filename)))
            {
                MD5 hasher = MD5.Create();
                byte[] hashBytes = hasher.ComputeHash(file);
                return BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
        }

        /// <summary>
        /// Calculates an MD5 hash of the file specified by <paramref name="filename"/>
        /// </summary>
        /// <param name="filename">The file to hash</param>
        /// <returns>The MD5 hash of the file</returns>
        [Pure]
        public async Task<string> CalculateMd5HashAsync(string filename)
        {
            return await Task.Run(() => CalculateMd5Hash(filename));
        }

        #endregion

    }
}
