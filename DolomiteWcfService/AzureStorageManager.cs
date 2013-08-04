﻿using System;
using System.Diagnostics;
using System.IO;
using DolomiteWcfService.Threads;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using System.Collections.Generic;
using System.Linq;

namespace DolomiteWcfService
{
    class AzureStorageManager
    {
        /// <summary>
        /// Internal instance of the blob client
        /// </summary>
        private CloudBlobClient BlobClient { get; set; }

        #region Singleton Instance Code

        private static AzureStorageManager _instance;

        /// <summary>
        /// Singleton instance of the Azure Storage manager
        /// </summary>
        public static AzureStorageManager Instance
        {
            get { return _instance ?? (_instance = new AzureStorageManager()); }
        }

        /// <summary>
        /// Singleton constructor for the AzureStorageManager
        /// </summary>
        private AzureStorageManager() 
        {
            // Create a client for accessing the Azure storage
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = account.CreateCloudBlobClient();
        }

        #endregion

        #region Store Methods

        /// <summary>
        /// Stores the given stream into a block blob in the given storage
        /// container.
        /// </summary>
        /// <param name="fileName">The path for the file to be stored</param>
        /// <param name="containerName">The name of the container to store the blob in</param>
        /// <param name="bytes">A stream of the bytes to store</param>
        public void StoreBlob(string fileName, string containerName, Stream bytes)
        {
            Trace.TraceInformation("Attempting to upload block blob '{0}' to container '{1}'", fileName, containerName);
            try
            {
                // Grab the container that is being used
                CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
                
                // Grab the 
                CloudBlockBlob block = container.GetBlockBlobReference(fileName);
                block.UploadFromStream(bytes);
                Trace.TraceInformation("Successfully stored block blob {0}", fileName);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to upload block {0}: {1}", fileName, e.Message);
                throw;
            }
        }

        /// <summary>
        /// Start the upload of a blob to the blob storage
        /// </summary>
        /// <param name="fileName">Target name of the file in Azure</param>
        /// <param name="containerNameKey">KEY to the container name</param>
        /// <param name="bytes">The bytes to upload to Azure</param>
        /// <param name="callback">The callback to perform when completed</param>
        /// <param name="state">The object to pass to the callback</param>
        /// @TODO Should replace the type of the asynchronous state to some inheritance thingy
        public void StoreBlobAsync(string fileName, string containerNameKey, Stream bytes, AsyncCallback callback, TrackOnboarding.AsynchronousState state)
        {
            try
            {
                // Grab the container that is being used
                if (Properties.Settings.Default[containerNameKey] == null)
                {
                    throw new InvalidDataException("Track storage container key not set in settings.");
                }
                string containerName = Properties.Settings.Default[containerNameKey].ToString();
                CloudBlobContainer container = BlobClient.GetContainerReference(containerName);

                Trace.TraceInformation("Attempting asynchronous upload of block blob '{0}' to container '{1}'", fileName, containerName);

                // Grab the 
                CloudBlockBlob block = container.GetBlockBlobReference(fileName);
                state.Blob = block;
                block.BeginUploadFromStream(bytes, callback, state);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to start upload of block blob {0}: {1}", fileName, e.Message);
                throw;
            }
        }

        #endregion

        #region Retrieve Methods

        /// <summary>
        /// Retrieve a specific track at a the given path.
        /// </summary>
        /// <param name="path">The path to find the track</param>
        /// <param name="containerName">Name of the container to retrieve the blob from</param>
        /// <returns>
        /// A stream that represents contains the track. 
        /// The position will be reset to the beginning.
        /// </returns>
        public Stream GetBlob(string containerName, string path)
        {
            Trace.TraceInformation("Attempting to retrieve '{0}' from container '{1}'", path, containerName);
            try
            {
                // Retreive a reference to the track container
                CloudBlobContainer container = BlobClient.GetContainerReference(containerName);

                // Retreive the track into a memory stream
                MemoryStream memStream = new MemoryStream();
                CloudBlockBlob blob = container.GetBlockBlobReference(path);
                if (!blob.Exists())
                {
                    throw new FileNotFoundException(String.Format("Failed to find blob {0} in container {1}", path, container));
                }
                blob.DownloadToStream(memStream);
                memStream.Position = 0;
                return memStream;
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to retrieve blob {0}: {1}", path, e.Message);
                throw;
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Attempts to create a container with the specified.
        /// </summary>
        /// <param name="containerName">Name of the container to create</param>
        public void InitializeContainer(string containerName)
        {
            // Check for the existence of the container
            CloudBlobContainer container = BlobClient.GetContainerReference(containerName);
            if (container.Exists())
                return;

            // Container does not exist. Create it
            try
            {
                Trace.TraceWarning("Container '{0}' does not exist. Attempting to create...", container.Name);
                container.Create();
                Trace.TraceInformation("Container '{0}' created successfully", container.Name);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to create container '{0}': {1}", container.Name, e.Message);
                throw;
            }
        }

        #endregion
    }
}
