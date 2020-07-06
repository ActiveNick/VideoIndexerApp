using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.Media.Core;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

// TODO: Extract Video Indexer API code into a separate .NET Standard Video Indexer client library
// TODO: Add proper exception handling
// TODO: Add a full ListView to show al the video results returned
// TODO: Add functionality to browse all the video segments that were indexed for each video
// TODO: Add upload functionality to add videos to an account and trigger indexing in the cloud

namespace VideoIndexerClient
{
    /// <summary>
    /// Simple demo UI used to navigate content in a specific Azure Video Indexer account
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string apiUrl = "https://api.videoindexer.ai";
        // TODO: move the location to a dropdown list in the UI
        string location = "eastus2"; // replace with the account's location, or with “trial” if this is a trial account
        string accountId;

        // User Session Objects
        HttpClient client;          // Client object used to make requests against the Video Indexer API for a specific account
        string accountAccessToken;  // Access token required for all requests made against the Video Indexer account

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Connects to Azure Video Indexer using a user account and API KEY
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            string msg;

            // Adds TLS 1.2 to the current security protocol for web service calls
            ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls12;

            // Azure Video Indexer Account ID and API KEY are set in the UI
            accountId = txtAccountID.Text.Trim();
            if (accountId.Length == 0)
            {
                msg = "Please verify that you entered a valid Azure Video Indexer account ID and try again.";
                return;
            }
            string apiKey = txtAPIkey.Text.Trim();
            if (apiKey.Length == 0)
            {
                msg = "Please verify that you entered a valid Azure Video Indexer API key and try again.";
                return;
            }

            // Create the http client to be used for the duration of the user session
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            // Obtain account access token to be used for all video indexer calls for this user session
            var accountAccessTokenRequestResult = client.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/AccessToken?allowEdit=true").Result;
            accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            // TODO: Add a mechanism to refresh the account access token if it expires (how long does it last?)

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            msg = "Video Indexer connection authorized: " + accountAccessToken;
            PostStatusMessage(msg.Substring(0, 100) + "...");
            Debug.WriteLine(msg);

            btnGetVideos.IsEnabled = true;
        }

        /// <summary>
        /// Retrieves videos stored in Azure Video Indexer for a specific account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetVideos_Click(object sender, RoutedEventArgs e)
        {
            var searchRequestResult = client.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/Search?accessToken={accountAccessToken}").Result;
            if (searchRequestResult.IsSuccessStatusCode)
            {
                // Only the first page of results is returned
                // TODO: Add paging support to retrieve subsequent videos for a search query
                VideoIndexerSearchResults searchResult = JsonConvert.DeserializeObject<VideoIndexerSearchResults>(searchRequestResult.Content.ReadAsStringAsync().Result);
                string msg = string.Format("Search results found: {0} video entries.", searchResult.VideoResults.Count());
                PostStatusMessage(msg);
                Debug.WriteLine(msg);

                if (searchResult.VideoResults.Count() > 0)
                {
                    // Use the first video returned in the list and load it in the media player
                    // Note that videos aren't sorted other than by the order in which they were uploaded
                    DisplayVideoData(searchResult.VideoResults[0]);
                }
            }
        }

        /// <summary>
        /// Access a specific video in a VI account and load it in the Media Player control for playback
        /// </summary>
        /// <param name="video">The unique VI ID of the requested video</param>
        private void DisplayVideoData(VideoResult video)
        {
            // Retrieve the thumbnail for that video
            var searchRequestResult = client.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/{video.id}/Thumbnails/{video.thumbnailId}?accessToken={accountAccessToken}").Result;
            if (searchRequestResult.IsSuccessStatusCode)
            {
                var thumbnail = (MemoryStream)searchRequestResult.Content.ReadAsStreamAsync().Result;
                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes((byte[])thumbnail.ToArray());
                        writer.StoreAsync().GetResults();
                    }
                    var image = new BitmapImage();
                    image.SetSource(ms);
                    // The poster is only shown while the video is loading. Once loaded, the video player will show the first frame (which could be black)
                    // TODO: Find a way to override the first frame in the video player to always show the thumbnail instead
                    videoPlayer.PosterSource = image;
                }
            }

            // Retrieve the source url for a given video ID
            searchRequestResult = client.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/{video.id}/SourceFile/DownloadUrl?accessToken={accountAccessToken}").Result;
            if (searchRequestResult.IsSuccessStatusCode)
            {
                var address = JsonConvert.DeserializeObject<string>(searchRequestResult.Content.ReadAsStringAsync().Result);
                // Initialize the video player control for playback of that video
                videoPlayer.Source = MediaSource.CreateFromUri(new Uri(address));
            }
        }

        /// <summary>
        /// Displays a status message in a log-type dump label in the UI
        /// </summary>
        /// <param name="msg"></param>
        private void PostStatusMessage(string msg)
        {
            lblResult.Text += msg + Environment.NewLine;
        }
    }
}
