// <copyright file="Program.cs" company="Microsoft Corp">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>

namespace VideoIndexerClient.DemoConsole
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Microsoft.Extensions.Configuration;
    using VideoIndexerLibrary;

    /// <summary>
    /// Sample console app which demonstrates how to consume the Video Indexer Library.
    /// </summary>
    public static class Program
    {
        // Constants
        // private const string VIResultUnknownFace = "UNKNOWN";
        private const string UIMessageWelcome = "Hello Video Indexer!";
        private const string UIMessageUploadCompleted = "Operation completed";
        private const string UIErrorArgumentMissingVideoName = "Upload Error: Name of video to upload is missing (-name).";

        // Configuration settings retrieved from appsettings.json
        private static IConfigurationRoot config;

        // Account credentials
        private static string accountId;
        private static string apiKey;
        private static string accountLocation;
        private static string blobAccount;
        private static string blobConnectionString;

        // User Session Objects & Variables
        private static VideoIndexer client;        // Client object used to make requests against the Video Indexer API for a specific account
        private static string accountAccessToken;  // Access token required for all requests made against the Video Indexer account

        /// <summary>
        /// Main routine and entry point of this console app.
        /// </summary>
        /// <param name="args">Command line arguments supplied in an array of strings.
        /// This will be parsed using the Microsoft.Extensions.Configuration.CommandLine package.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine(UIMessageWelcome);

            // Supported switches for the command line parameters.
            // TODO: Still need to figure out the proper workings of single dash switches vs double dash alias switches.
            var switchMappings = new Dictionary<string, string>()
             {
                 { "-downloadthumbnails", "downloadthumbnailsvideoid" },
                 { "-dt", "downloadthumbnailsvideoid" },
                 { "-upload", "upload" },
                 { "-up", "upload" },
                 { "-name", "name" },
                 { "-description", "description" },
                 { "-desc", "description" },
                 { "-savetofolder", "savefolder" },
                 { "-savetocloud", "savecontainer" },
             };

            // The configuration is a combination of command line arguments and appsettings.json for all the Azure secrets
            config = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .AddCommandLine(args, switchMappings)
                        .Build();

            // Retrieve Video Indexer account settings and connect
            accountId = config["VIDEOINDEXER_ACCOUNTID"];
            apiKey = config["VIDEOINDEXER_APIKEY"];
            accountLocation = config["VIDEOINDEXER_REGION"];
            ConnectToAccount(accountId, apiKey, accountLocation).Wait();

            // Retrieve Azure Blob Storage account settings and connect
            blobAccount = config["AZURE_BLOB_STORAGE_NAME"];
            blobConnectionString = config["AZURE_BLOB_STORAGE_CONNECTION_STRING"];

            // Download all shot thumbnails from a VI video to local jpeg files
            if (config["downloadthumbnailsvideoid"]?.Length > 0)
            {
                SaveThumbnailsFromVideoShots(config["downloadthumbnailsvideoid"]).Wait();
            }

            // Upload a video from a local file
            if (config["upload"]?.Length > 0)
            {
                // The video name is required.
                string name = config["name"];
                if (name.Length == 0)
                {
                    Console.WriteLine(UIErrorArgumentMissingVideoName);
                    return;
                }

                // The video description is optional.
                string desc = config["description"];

                // string file = "C:\\Users\\Nick\\OneDrive\\Videos\\Microsoft\\The New Microsoft Band  Live Healthier and Achieve More-FullHD.mp4";
                string file = config["upload"];

                // Upload the video
                UploadVideoFromFile(name, desc, file).Wait();
            }

#if DEBUG
            // End of program, wait for the user to press ENTER when debugging to preserve the console output
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
#endif
        }

        /// <summary>
        /// Connects to Azure Video Indexer using a the credentials retrieved from appsettings.json .
        /// </summary>
        /// <param name="accountid">Video Indexer user account ID.</param>
        /// <param name="apikey">API key for this VI account.</param>
        /// <param name="location">Azure region where this VI account is located.</param>
        /// <returns>Status message about the connection to VI.</returns>
        private static async Task ConnectToAccount(string accountid, string apikey, string location)
        {
            string msg;

            try
            {
                // Azure Video Indexer Account ID and API KEY are set in the UI
                if (accountid.Length == 0)
                {
                    msg = "Account ID is missing. Please verify that you entered a valid Azure Video Indexer account ID and try again.";
                    Console.WriteLine(msg);
                    return;
                }

                if (apikey.Length == 0)
                {
                    msg = "API key is missing. Please verify that you entered a valid Azure Video Indexer API key and try again.";
                    Console.WriteLine(msg);
                    return;
                }

                client = new VideoIndexer();

                // TODO: Add a mechanism to refresh the account access token if it expires (how long does it last?)
                accountAccessToken = await client.GetAccountAccessTokenAsync(accountid, apikey, location).ConfigureAwait(false);
                if (accountAccessToken != null)
                {
                    msg = "Video Indexer connection authorized: " + accountAccessToken.Substring(0, 80) + "...";
                }
                else
                {
                    msg = "Authorization error. Please verify that you entered valid Azure Video Indexer credentials and try again.";
                }
            }
            catch (WebException exc)
            {
                msg = "Video Indexer API error. Please verify that you entered valid Azure Video Indexer credentials and try again. Error:"
                      + Environment.NewLine + exc.Message;
            }
            catch (Exception exc)
            {
                msg = "General authorization error. Please verify that you entered valid Azure Video Indexer credentials and try again. Error:"
                      + Environment.NewLine + exc.Message;
            }

            Console.WriteLine(msg);
        }

        /// <summary>
        /// Uploads a video to VI based on a local file.
        /// </summary>
        /// <param name="name">Name of video to be stored in VI.</param>
        /// <param name="desc">Description of the video.</param>
        /// <param name="path">Full path and file name of the local video file to upload to VI.</param>
        /// <returns>Awaitable task.</returns>
        private static async Task UploadVideoFromFile(string name, string desc, string path)
        {
            string msg = $"Uploading video: {name}. Please wait...";
            Console.WriteLine(msg);
            Uri fileUri = new Uri(path);

            msg = await client.UploadVideoAsync(name, false, desc, fileUri, UploadMethod.LocalFile).ConfigureAwait(false);
            Console.WriteLine($"{UIMessageUploadCompleted}: {msg}");
        }

        /// <summary>
        /// Retrieve all thumbnails from a video in VI and upload them to blob storage.
        /// Currently cycles through the thumbnails for every instance, in every keyframe, in every shot.
        /// </summary>
        /// <param name="videoid">The VI ID of the video to be parsed for thumbnails.</param>
        /// <returns>Awaitable task.</returns>
        private static async Task SaveThumbnailsFromVideoShots(string videoid)
        {
            string msg;

            try
            {
                // Tracking the start time for performance analytics
                DateTime startTimestamp = DateTime.Now;
                msg = $"[{startTimestamp.ToShortTimeString()}] Saving all shot thumbnails for video id: {videoid}. Please wait...";
                Console.WriteLine(msg);

                int thumbcount = 0;

                // Retrieve all the insights from a given video to parse them for thumbnails
                var insights = await client.GetInsightsAsync(videoid).ConfigureAwait(false);
                if (insights != null && insights.Videos.Count > 0)
                {
                    // Initialization for saving thumbnails to a local file folder
                    string foldername = null;
                    if (config["savefolder"]?.Length > 0)
                    {
                        // Create the selected director if it doesn't exist, the video ID is used as a subfolder
                        foldername = config["savefolder"].TrimEnd(new char[] { '/', '\\' });
                        DirectoryInfo folder = Directory.CreateDirectory(Path.Combine(foldername, videoid));
                        foldername = folder.FullName;
                    }

                    // Initialization for blob storage upload
                    // The container name is provided as a command line parameter
                    BlobContainerClient containerClient = null;
                    if (config["savecontainer"]?.Length > 0)
                    {
                        if (blobAccount.Length > 0 && blobConnectionString.Length > 0)
                        {
                            // Create a BlobServiceClient object which will be used to create a container client
                            BlobServiceClient blobServiceClient = new BlobServiceClient(blobConnectionString);

                            if (blobServiceClient != null)
                            {
                                // Create the container and return a container client object. The video ID is used to aggregate thumbnails by video.
                                // TODO: Do we need to insure uniqueness of the container name to be created/used in blob storage?
                                containerClient = blobServiceClient.GetBlobContainerClient($"{config["savecontainer"]}-{videoid}".ToLowerInvariant());
                                await containerClient.CreateIfNotExistsAsync().ConfigureAwait(false);

                                if ((await (containerClient?.ExistsAsync()).ConfigureAwait(false)) == true)
                                {
                                    // Prepare metadata to augment the blob container
                                    var blobMetadata = new Dictionary<string, string>()
                                                        {
                                                            { "videoid", videoid },
                                                            { "videoname", insights.Name },
                                                        };
                                    await containerClient.SetMetadataAsync(blobMetadata).ConfigureAwait(false);
                                }
                            }
                        }
                    }

                    // Cycle through each video in the playlist returned by VI
                    foreach (Video video in insights.Videos)
                    {
                        // Cycle through each shot indexed by VI
                        Console.WriteLine($"No of shots in video: {insights.Videos[0].Insights.Shots.Count}");
                        foreach (Shot shot in video.Insights.Shots)
                        {
                            // Cycle through each keyframe in every shot
                            foreach (Keyframe keyframe in shot.KeyFrames)
                            {
                                // Cycle through each thumbnail instance in a keyframe
                                foreach (Instance instance in keyframe.Instances)
                                {
                                    // Not all instances are guaranteed to have a thumbnail
                                    if (instance.ThumbnailId.Length > 0)
                                    {
                                        // Retrieve the thumbnail for that video
                                        using (MemoryStream thumbnail = await client.GetThumbnailAsync(videoid, instance.ThumbnailId).ConfigureAwait(false) as MemoryStream)
                                        {
                                            if (thumbnail != null && thumbnail.Length > 0)
                                            {
                                                using (var image = new Bitmap(thumbnail))
                                                {
                                                    string thumbfile = $"{videoid}_{shot.Id}_{keyframe.Id}_{thumbcount}.jpg";

                                                    // Saving the thumbnail to a folder
                                                    if (foldername.Length > 0)
                                                    {
                                                        Console.WriteLine($"Saving thumbnail image file: {thumbfile}");
                                                        image.Save(Path.Combine(foldername, thumbfile));
                                                    }

                                                    // Saving the thumbnail to Azure blob storage
                                                    if (config["savecontainer"]?.Length > 0)
                                                    {
                                                        if (containerClient != null)
                                                        {
                                                            // Get a reference to a blob
                                                            BlobClient blobClient = containerClient.GetBlobClient(thumbfile);

                                                            Console.WriteLine($"Saving thumbnail image to blob storage: {thumbfile}");

                                                            // Open the file and upload its data
                                                            thumbnail.Position = 0;
                                                            await blobClient.UploadAsync(thumbnail, true).ConfigureAwait(false);

                                                            if ((await (blobClient?.ExistsAsync()).ConfigureAwait(false)) == true)
                                                            {
                                                                // Prepare metadata to augment the blob
                                                                var blobMetadata = new Dictionary<string, string>()
                                                                {
                                                                    { "videoid", videoid },
                                                                    { "videoname", insights.Name },
                                                                    { "shotid", shot.Id.ToString(CultureInfo.InvariantCulture) },
                                                                    { "keyframeid", keyframe.Id.ToString(CultureInfo.InvariantCulture) },
                                                                    { "starttime", instance.Start },
                                                                };
                                                                await blobClient.SetMetadataAsync(blobMetadata).ConfigureAwait(false);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            thumbnail.Close();
                                        }
                                    }

                                    thumbcount++;
                                }
                            }
                        }
                    }
                }

                // Reporting elapsed time for performance analytics
                DateTime endTimestamp = DateTime.Now;
                msg = $"[Elapsed time: {endTimestamp - startTimestamp:c}] Operation completed for video {videoid}. Total of {thumbcount} thumbnails extracted.";
                Console.WriteLine(msg);
            }

            // Exception likely raised by Video Indexer API calls.
            catch (WebException exc)
            {
                msg = $"Video Indexer API error retrieving indexed insights for video {videoid} in account {accountId}:"
                      + Environment.NewLine + exc.Message;
            }

            // Exception likely raised by calls to Azure Blob storage.
            catch (Azure.RequestFailedException exc)
            {
                msg = $"Azure Blob Storage error while saving thumbnails for video {videoid} in account {accountId}:"
                      + Environment.NewLine + exc.Message;
            }

            // Final fallback to report generic exceptions not captured above.
            // TODO: Add more specific exceptions to be captured above based on scenarios that crop up during testing.
            catch (Exception exc)
            {
                msg = $"General error retrieving indexed insights for video {videoid} in account {accountId}:"
                      + Environment.NewLine + exc.Message;
                Console.WriteLine(msg);
            }
        }
    }
}
