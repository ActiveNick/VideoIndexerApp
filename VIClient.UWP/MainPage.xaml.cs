// <copyright file="MainPage.xaml.cs" company="Microsoft Corp">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>

// TODO: Add a full ListView to show al the video results returned [UI]
// TODO: Add functionality to browse all the video segments that were indexed for each video [UI]
// TODO: Add functionality to browse through all instances of faces, keywords, brands, named locations, etc. [UI]
// TODO: Add upload functionality to add videos to an account and trigger indexing in the cloud [UI+Lib]
// TODO: Add search capability to look for instances of words in the script [UI+Lib]
namespace VideoIndexerClient
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using VideoIndexerLibrary;
    using Windows.Media.Core;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Simple demo UI used to navigate content in a specific Azure Video Indexer account.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Constants
        private const string VIResultUnknownFace = "UNKNOWN";
        private const string UIErrorMessage = "File extension is incorrect.";

        // TODO: move the location to a dropdown list in the UI
        private string accountId;
        private string apiKey;
        private string accountLocation;

        // User Session Objects & Variables
        private VideoIndexer client;        // Client object used to make requests against the Video Indexer API for a specific account
        private string accountAccessToken;  // Access token required for all requests made against the Video Indexer account

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            this.txtAccountID.Text = config["VIDEOINDEXER_ACCOUNTID"];
            this.txtAPIkey.Text = config["VIDEOINDEXER_APIKEY"];
            this.txtRegion.Text = config["VIDEOINDEXER_REGION"];    // TODO: Replace the Region textbox with a UI droplist
        }

        /// <summary>
        /// Allows a user to select a file using a FilePicker based on preset extensions.
        /// </summary>
        /// <param name="types">Array of strings containing file extensions to filter the FilePicker.</param>
        /// <returns>The file selected by the user as a Windows StorageFile object.</returns>
        private static async Task<StorageFile> OpenLocalFile(params string[] types)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            Regex typeReg = new Regex(@"^\.[a-zA-Z0-9]+$");
            foreach (var type in types)
            {
                if (type == "*" || typeReg.IsMatch(type))
                {
                    picker.FileTypeFilter.Add(type);
                }
                else
                {
                    throw new InvalidCastException(UIErrorMessage);
                }
            }

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                return file;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Connects to Azure Video Indexer using a user account and API KEY.
        /// </summary>
        /// <param name="sender">The XAML control which raised the event.</param>
        /// <param name="e">Event parameters.</param>
        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            string msg;

            try
            {
                // Azure Video Indexer Account ID and API KEY are set in the UI
                this.accountId = this.txtAccountID.Text.Trim();
                if (this.accountId.Length == 0)
                {
                    msg = "Account ID is missing. Please verify that you entered a valid Azure Video Indexer account ID and try again.";
                    this.PostStatusMessage(msg);
                    return;
                }

                this.apiKey = this.txtAPIkey.Text.Trim();
                if (this.apiKey.Length == 0)
                {
                    msg = "API key is missing. Please verify that you entered a valid Azure Video Indexer API key and try again.";
                    this.PostStatusMessage(msg);
                    return;
                }

                this.accountLocation = this.txtRegion.Text.Trim();
                if (this.accountLocation.Length == 0)
                {
                    msg = "Account region is missing. Please verify that you entered a valid Azure Video Indexer account region and try again.";
                    this.PostStatusMessage(msg);
                    return;
                }

                this.client = new VideoIndexer();

                // TODO: Add a mechanism to refresh the account access token if it expires (how long does it last?)
                this.accountAccessToken = await this.client.GetAccountAccessTokenAsync(this.accountId, this.apiKey, this.accountLocation).ConfigureAwait(false);
                if (this.accountAccessToken != null)
                {
                    msg = "Video Indexer connection authorized: " + this.accountAccessToken.Substring(0, 100) + "...";

                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        this.btnGetVideos.IsEnabled = true;
                        this.btnUploadVideo.IsEnabled = true;
                    });
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

            this.PostStatusMessage(msg);
        }

        /// <summary>
        /// Retrieves videos stored in Azure Video Indexer for a specific account.
        /// </summary>
        /// <param name="sender">The XAML control which raised the event.</param>
        /// <param name="e">Event parameters.</param>
        private async void BtnGetVideos_Click(object sender, RoutedEventArgs e)
        {
            string msg;

            try
            {
                VideoIndexerSearchResults searchResult = await this.client.GetVideosAsync().ConfigureAwait(false);
                if (searchResult != null && searchResult.Videos.Count > 0)
                {
                    msg = $"Search results found: {searchResult.Videos.Count} video entries.";
                    this.PostStatusMessage(msg);

                    // Use the first video returned in the list and load it in the media player
                    // Note that videos aren't sorted other than by the order in which they were uploaded
                    this.DisplayVideoData(searchResult.Videos[0]);

                    // Get insights from indexer for the first video in the list
                    this.DisplayInsights(searchResult.Videos[0].Id);
                }
            }
            catch (WebException exc)
            {
                msg = $"Video Indexer API error retrieving videos for account {this.accountId}:"
                      + Environment.NewLine + exc.Message;
            }
            catch (Exception exc)
            {
                msg = $"General error retrieving videos for account {this.accountId}:"
                      + Environment.NewLine + exc.Message;
                this.PostStatusMessage(msg);
            }
        }

        /// <summary>
        /// Access a specific video in a VI account and load it in the Media Player control for playback.
        /// </summary>
        /// <param name="video">The unique VI ID of the requested video.</param>
        private async void DisplayVideoData(Video video)
        {
            string msg;

            try
            {
                // Retrieve the thumbnail for that video
                MemoryStream thumbnail = await this.client.GetThumbnailAsync(video.Id, video.ThumbnailId).ConfigureAwait(false) as MemoryStream;
                if (thumbnail != null)
                {
                    using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                    {
                        using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                        {
                            writer.WriteBytes((byte[])thumbnail.ToArray());
                            await writer.StoreAsync();
                        }

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            var image = new BitmapImage();
                            await image.SetSourceAsync(ms);

                            // The poster is only shown while the video is loading. Once loaded, the video player will show the first frame (which could be black)
                            // TODO: Find a way to override the first frame in the video player to always show the thumbnail instead
                            this.videoPlayer.PosterSource = image;
                        });
                    }
                }

                // Retrieve the source url for a given video ID
                string address = await this.client.GetVideoUrlAsync(video.Id).ConfigureAwait(false);
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    // Initialize the video player control for playback of that video
                    this.videoPlayer.Source = MediaSource.CreateFromUri(new Uri(address));
                });
                this.PostStatusMessage($"Current Video Name: {video.Name}");
            }
            catch (WebException exc)
            {
                msg = $"Video Indexer API error retrieving thumbnail image and url for video {video.Id} in account {this.accountId}:"
                      + Environment.NewLine + exc.Message;
            }
            catch (Exception exc)
            {
                msg = $"General error retreiveing thumbnail image and url for video {video.Id} in account {this.accountId}:"
                      + Environment.NewLine + exc.Message;
                this.PostStatusMessage(msg);
            }
        }

        /// <summary>
        /// Get the VI index insights data for a specific video.
        /// </summary>
        /// <param name="videoId">The unique VI ID of the video fo which we need to retrieve indexed insights.</param>
        private async void DisplayInsights(string videoId)
        {
            string msg;

            try
            {
                var insights = await this.client.GetInsightsAsync(videoId).ConfigureAwait(false);
                if (insights != null)
                {
                    // Post summary of faces
                    this.PostStatusMessage($" No of faces: {insights.SummarizedInsights.Faces.Count}");
                    int unknowns = 0;
                    foreach (var face in insights.SummarizedInsights.Faces)
                    {
                        // This check is only valid when using Video Indexer within an English locale
                        // TODO: Add support for other cultures based on what is supported by Video Indexer
                        if (face.Name.Contains(VIResultUnknownFace, System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            unknowns++;
                        }
                        else
                        {
                            this.PostStatusMessage($"  Face: {face.Name} (occurrences: {face.Appearances.Count})");
                        }
                    }

                    if (unknowns > 0)
                    {
                        this.PostStatusMessage($"  Unknown Faces Counted: {unknowns}");
                    }

                    // Post summary of brands
                    this.PostStatusMessage($" No of brands: {insights.SummarizedInsights.Brands.Count}");
                    foreach (var brand in insights.SummarizedInsights.Brands)
                    {
                        this.PostStatusMessage($"  Brand: {brand.Name} (occurrences: {brand.Appearances.Count})");
                    }

                    // Post summary of keywords
                    this.PostStatusMessage($" No of keywords: {insights.SummarizedInsights.Keywords.Count}");
                    foreach (var keyword in insights.SummarizedInsights.Keywords)
                    {
                        this.PostStatusMessage($"  Keyword: {keyword.Name} (occurrences: {keyword.Appearances.Count})");
                    }
                }
            }
            catch (WebException exc)
            {
                msg = $"Video Indexer API error retrieving indexed insights for video {videoId} in account {this.accountId}:"
                      + Environment.NewLine + exc.Message;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exc)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                msg = $"General error retrieving indexed insights for video {videoId} in account {this.accountId}:"
                      + Environment.NewLine + exc.Message;
                this.PostStatusMessage(msg);
            }
        }

        /// <summary>
        /// Uploads a video from a local file selected from a file picker.
        /// </summary>
        private async void UploadVideoFromFile()
        {
            // TODO: There is currently an issue with file access permissions in this application.
            // UWP apps cannot access the full file system but we need to be able to select video files anywhere for upload.
            // The broadFileSystemAccess capability doesn't seem to be picked up, or requires the use of Windows.Storage.StorageFile.
            // But we cannot use this Windows-specific class in the .NET Standard library since it's meant to be cross-platform .NET code.
            // Access to the video upload in the UWP app is disabled in the meantime until fixed.

            // TODO: Add UI elements to provide metadata via text fields
            string name = "Microsoft Band - Live Healthier and Achieve More";
            string desc = "Promotional video about Microsoft Band";
            StorageFile file = await OpenLocalFile(".mp4").ConfigureAwait(false);

            await this.client.UploadVideoAsync(name, false, desc, new System.Uri(file.Path), UploadMethod.LocalFile).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a video from a publicly available url.
        /// </summary>
        private async void UploadVideoFromUrl()
        {
            System.Uri videoUrl = new Uri("https://3nbs7a.bn.files.1drv.com/y4mTX4B8iGIQsZiXLMDXnZ1FyBs14Joay_8vNMFGSTMbf04tcT28KLgEsdCOrG6ExzZqkewITXsFvh9vaUC8iaw2-Q6Eap-Mtz0hP3isR1fC2P5UUCzvc_Q3nkN0Sl43thS4bF7qTpo0uO2eXiL_UCKuREHNMFtlLpuIdZnvSRRFbKN5CZknKq0D58dL3s6qetwmkViKsId4CQsbuQO6fpBrQ/Microsoft%20Devices%20%20Do%20Great%20Things-4ZawV1mXlS8-FullHD.mp4");
            string name = "Microsoft Devices - Do Great Things";
            string desc = "Promotional video about Microsoft hardware devices";

            await this.client.UploadVideoAsync(name, false, desc, videoUrl, UploadMethod.PublicUrl).ConfigureAwait(false);
        }

        /// <summary>
        /// Displays a status message in a log-type dump label in the UI.
        /// </summary>
        /// <param name="msg">The message to be displayed in the UI result control.</param>
        /// <param name="debug">Set to true (default) if the message should be written to the debug console.</param>
        private async void PostStatusMessage(string msg, bool debug = true)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.lblResult.Text += msg + Environment.NewLine;
            });
            if (debug)
            {
                Debug.WriteLine(msg);
            }
        }

        private void BtnUploadVideo_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Fix a permissions issue tied to the UWP app not having permissions to access the file system.
            // Disabling the 'upload by local file' option in the meantime until it is solved.
            // this.UploadVideoFromFile();

            this.UploadVideoFromUrl();
        }
    }
}
