using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        string accountId = "...";
        string location = "eastus2"; // replace with the account's location, or with “trial” if this is a trial account
        string apiKey = "...";

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
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            // obtain account access token
            var accountAccessTokenRequestResult = client.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/AccessToken?allowEdit=true").Result;
            var accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");
        }
    }
}
