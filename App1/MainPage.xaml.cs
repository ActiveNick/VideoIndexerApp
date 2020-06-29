﻿using System;
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

namespace VideoIndexerClient
{
    /// <summary>
    /// Simple demo UI used to navigate content in a specific Azure Video Indexer account
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string apiUrl = "https://api.videoindexer.ai";
        string accountId;
        // TODO: move the location to a dropdown list in the UI
        string location = "eastus2"; // replace with the account's location, or with “trial” if this is a trial account
        string apiKey;

        // Session objects
        HttpClient client;          // Client object used to make requests against the Video Indexer API for a specific account
        string accountAccessToken;  // Access token required for all requests made against the Video Indexer account

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            string msg;

            ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls12;

            accountId = txtAccountID.Text.Trim();
            if (accountId.Length == 0)
            {
                msg = "Please verify that you entered a valid Azure Video Indexer account ID and try again.";
                return;
            }
            apiKey = txtAPIkey.Text.Trim();
            if (apiKey.Length == 0)
            {
                msg = "Please verify that you entered a valid Azure Video Indexer API key and try again.";
                return;
            }

            // create the http client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            // obtain account access token
            var accountAccessTokenRequestResult = client.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/AccessToken?allowEdit=true").Result;
            accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            msg = "Video Indexer connection authorized: " + accountAccessToken;
            PostStatusMessage(msg.Substring(0, 100) + "...");
            Debug.WriteLine(msg);

            btnGetVideos.IsEnabled = true;
        }

        private void btnGetVideos_Click(object sender, RoutedEventArgs e)
        {
            var searchRequestResult = client.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/Search?accessToken={accountAccessToken}").Result;
            VideoIndexerSearchResults searchResult = JsonConvert.DeserializeObject<VideoIndexerSearchResults>(searchRequestResult.Content.ReadAsStringAsync().Result);
            string msg = string.Format("Search results found: {0} video entries.", searchResult.VideoResults.Count());
            PostStatusMessage(msg);
            Debug.WriteLine(msg);

            if (searchResult.VideoResults.Count() > 0)
            {
                DisplayVideoData(searchResult.VideoResults[0]);
            }
        }

        private void DisplayVideoData(VideoResult video)
        {
            var searchRequestResult = client.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/{video.id}/SourceFile/DownloadUrl?accessToken={accountAccessToken}").Result;
            //var searchResult = searchRequestResult.Content.ReadAsStringAsync();
            string address = JsonConvert.DeserializeObject<string>(searchRequestResult.Content.ReadAsStringAsync().Result);
            videoPlayer.Source = MediaSource.CreateFromUri(new Uri(address));
        }

        private void PostStatusMessage(string msg)
        {
            lblResult.Text += msg + Environment.NewLine;
        }
    }
}
