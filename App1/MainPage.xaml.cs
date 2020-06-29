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

namespace VideoIndexerClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string apiUrl = "https://api.videoindexer.ai";
        string accountId = "insert-your-video-indexer-account-ID-here";
        string location = "eastus2"; // replace with the account's location, or with “trial” if this is a trial account
        string apiKey = "insert-your-video-indexer-secret-API-key-here";

        // Session objects
        HttpClient client;
        string accountAccessToken;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls12;

            // create the http client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            // obtain account access token
            var accountAccessTokenRequestResult = client.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/AccessToken?allowEdit=true").Result;
            accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            Debug.WriteLine("Video Indexer connection authorized: " + accountAccessToken);
        }

        private void btnGetVideos_Click(object sender, RoutedEventArgs e)
        {
            var searchRequestResult = client.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/Search?accessToken={accountAccessToken}").Result;
            var searchResult = searchRequestResult.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("");
            Debug.WriteLine("Search:");
            Debug.WriteLine(searchResult);
        }
    }
}
