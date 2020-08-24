// <copyright file="VideoIndexer.cs" company="Microsoft Corp">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>

namespace VideoIndexerLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// Library used to interact with the Video Indexer API.
    /// See https://api-portal.videoindexer.ai/ for more info about the API.
    /// Video Indexer developer documentation is located at
    /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/.
    /// </summary>
    public class VideoIndexer
    {
        // Error messages
        private const string VideoIndexerErrorMessageEmptyVideoName = "The video name cannot be empty.";
        private const string VideoIndexerErrorMessageInvalidUrl = "The video url is invalid.";

        // Video Indexer API client and url
        private const string VideoIndexerApiUrl = "https://api.videoindexer.ai";
        private static HttpClientHandler handler = new HttpClientHandler() { AllowAutoRedirect = false };
        private static HttpClient client = new HttpClient(handler);

        // Video Indexer Credentials
        private string accountId;           // Video Indexer Account ID
        private string accountLocation;     // Azure region
        private string accountAccessToken;  // Access token required for all requests made against the Video Indexer account

        /// <summary>
        /// Authorizes access to a Video Indexer account using an API subscription key.
        /// For more information on Subscribing to the Video Indexer API, see the docs at
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-use-apis#subscribe-to-the-api.
        /// </summary>
        /// <param name="accountid">The Video Indexer ID of the user account to use for this app session.</param>
        /// <param name="apiKey">The Video Indexer API key required for all API calls to this VI account.</param>
        /// <param name="location">The Azure region where this Video Indexer account is hosted.</param>
        /// <returns>Account access token in string format.</returns>
        public async Task<string> GetAccountAccessTokenAsync(string accountid, string apiKey, string location)
        {
            // Adds TLS 1.2 to the current security protocol for web service calls
            ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls12;

            if (accountid?.Length == 0)
            {
                Debug.WriteLine("Account ID is missing. Please verify that you entered a valid Azure Video Indexer account ID and try again.");
                return null;
            }

            if (apiKey?.Length == 0)
            {
                Debug.WriteLine("API key is missing. Please verify that you entered a valid Azure Video Indexer API key and try again.");
                return null;
            }

            if (location?.Length == 0)
            {
                Debug.WriteLine("Location is missing. Please verify that you entered a valid Azure region name and try again.");
                return null;
            }

            // Create the http client to be used for the duration of the user session
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

            // Obtain account access token to be used for all video indexer calls for this user session
            Uri requestUri = new Uri($"{VideoIndexerApiUrl}/auth/{location}/Accounts/{accountid}/AccessToken?allowEdit=true");
            var accountAccessTokenRequestResult = await client.GetAsync(requestUri).ConfigureAwait(false);
            if (accountAccessTokenRequestResult.IsSuccessStatusCode)
            {
                this.accountId = accountid;
                this.accountLocation = location;

                // TODO: Add a mechanism to refresh the account access token if it expires. Expiration of all the access tokens in Video Indexer is one hour.
                this.accountAccessToken = (await accountAccessTokenRequestResult.Content.ReadAsStringAsync().ConfigureAwait(false)).Replace("\"", string.Empty);
            }
            else
            {
                Debug.WriteLine("Authorization error. Please verify that you entered valid Azure Video Indexer credentials and try again.");
                throw new WebException(accountAccessTokenRequestResult.ReasonPhrase);
            }

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            return this.accountAccessToken;
        }

        /// <summary>
        /// Retrieves videos currently stored in a specific Video Indexer account.
        /// </summary>
        /// <returns>Full object structure containing VideoIndexerSearchResults.</returns>
        public async Task<VideoIndexerSearchResults> GetVideosAsync()
        {
            Uri requestUri = new Uri($"{VideoIndexerApiUrl}/{this.accountLocation}/Accounts/{this.accountId}/Videos/Search?accessToken={this.accountAccessToken}");
            var searchRequestResult = await client.GetAsync(requestUri).ConfigureAwait(false);
            if (searchRequestResult.IsSuccessStatusCode)
            {
                // Only the first page of results is returned
                // TODO: Add paging support to retrieve subsequent videos for a search query
                VideoIndexerSearchResults searchResult = JsonConvert.DeserializeObject<VideoIndexerSearchResults>(await searchRequestResult.Content.ReadAsStringAsync().ConfigureAwait(false));

                return searchResult;
            }
            else
            {
                Debug.WriteLine($"An error has occurred while retrieving the videos for account ID {this.accountId}.");
                throw new WebException(searchRequestResult.ReasonPhrase);
            }
        }

        /// <summary>
        /// Retrieve the thumbnail for a specific video.
        /// </summary>
        /// <param name="videoId">The VI identifier of the video for which to retrieve a thumbnail.</param>
        /// <param name="thumbnailId">The ID of the thumbnail to retrieve.</param>
        /// <returns>Thumbnail data stored in a stream.</returns>
        public async Task<Stream> GetThumbnailAsync(string videoId, string thumbnailId)
        {
            Uri requestUri = new Uri($"{VideoIndexerApiUrl}/{this.accountLocation}/Accounts/{this.accountId}/Videos/{videoId}/Thumbnails/{thumbnailId}?accessToken={this.accountAccessToken}");
            var searchRequestResult = await client.GetAsync(requestUri).ConfigureAwait(false);
            if (searchRequestResult.IsSuccessStatusCode)
            {
                return await searchRequestResult.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            else
            {
                Debug.WriteLine($"An error has occurred while retrieving the thumbnail for account ID {this.accountId}, video ID {thumbnailId}.");
                throw new WebException(searchRequestResult.ReasonPhrase);
            }
        }

        /// <summary>
        /// Retrieve the playback url for a given video.
        /// </summary>
        /// <param name="videoId">The VI identifier of the video whose url is requested.</param>
        /// <returns>url in string format.</returns>
        public async Task<string> GetVideoUrlAsync(string videoId)
        {
            Uri requestUri = new Uri($"{VideoIndexerApiUrl}/{this.accountLocation}/Accounts/{this.accountId}/Videos/{videoId}/SourceFile/DownloadUrl?accessToken={this.accountAccessToken}");
            var searchRequestResult = await client.GetAsync(requestUri).ConfigureAwait(false);
            if (searchRequestResult.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<string>(await searchRequestResult.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            else
            {
                Debug.WriteLine($"An error has occurred while retrieving the url for ID {videoId}.");
                throw new WebException(searchRequestResult.ReasonPhrase);
            }
        }

        /// <summary>
        /// Get the VI index insights data for a specific video.
        /// </summary>
        /// <param name="videoId">The VI identifier of the video for which insights are retrieved from the index.</param>
        /// <returns>A VideoInsightsResult object via an async Task.</returns>
        public async Task<Playlist> GetInsightsAsync(string videoId)
        {
            Uri requestUri = new Uri($"{VideoIndexerApiUrl}/{this.accountLocation}/Accounts/{this.accountId}/Videos/{videoId}/Index?includeStreamingUrls&accessToken={this.accountAccessToken}");
            var insightsRequestResult = await client.GetAsync(requestUri).ConfigureAwait(false);

            if (insightsRequestResult.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Playlist>(await insightsRequestResult.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            else
            {
                Debug.WriteLine($"An error has occurred while retrieving indexed inasights video ID {videoId}.");
                throw new WebException(insightsRequestResult.ReasonPhrase);
            }
        }

        /// <summary>
        /// Method used to upload videos to a specific user account in Azure Video Indexer.
        /// Once a video is successfully uploaded, VI automatically indexes it to extract insights.
        /// When uploading your video based on the URL (preferred) the endpoint must be secured with TLS 1.2 (or higher).
        /// The upload size with the URL option is limited to 30GB.
        /// The upload size with the byte array option is limited to 2GB.
        /// Video Indexer has a max duration limit of 4 hours for a single file.
        /// Docs: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/upload-index-videos.
        /// </summary>
        /// <param name="name">The video's name. A name of the video must be no greater than 80 characters.</param>
        /// <param name="ispublic">Set to true for public videos, false for private.</param>
        /// <param name="desc">Description of the video.</param>
        /// <param name="videoUrl">The location of the video file. For the public url upload method, the URL needs to be encoded.</param>
        /// <param name="method">Specifies which method to use. See enum comments for details on available options.</param>
        /// <returns>HTTP request result.</returns>
        public async Task<string> UploadVideoAsync(string name, bool ispublic, string desc, System.Uri videoUrl, UploadMethod method)
        {
            // Validate parameters first
            if (name?.Length == 0)
            {
                throw new ArgumentException(VideoIndexerErrorMessageEmptyVideoName);
            }

            if (videoUrl != null && videoUrl.IsWellFormedOriginalString())
            {
                throw new ArgumentException(VideoIndexerErrorMessageInvalidUrl);
            }

            MultipartFormDataContent content = new MultipartFormDataContent();

            // The parameters of the video. This is only a partial list for now. For the full list, see:
            // https://api-portal.videoindexer.ai/docs/services/Operations/operations/Upload-Video.
            string queryParams = CreateQueryString(
                new Dictionary<string, string>()
                {
                    // Required. Should be given as parameter in URL query string or in Authorization header as Bearer token,
                    // and match the authorization scope of the call (Account, with Write).
                    // Note that Access tokens expire within 1 hour.
                    { "accessToken", this.accountAccessToken },

                    // The name of the video.
                    { "name", name },

                    // The video description.
                    { "description", desc },

                    // The video privacy mode. Allowed values: Private/Public.
                    { "privacy", ispublic ? "public" : "private" },

                    // A partition to partition videos by (used for searching a specific partition).
                    // TODO: Allow parameter to set the partition. Fixed for now.
                    { "partition", "partition" },
                });

            switch (method)
            {
                case UploadMethod.LocalFile:
                    // TODO: Implement gradual multipart upload to allow for larger files than 2GB.
                    // FileStream video = File.OpenRead(videoUrl.ToString());
                    // byte[] buffer =new byte[video.Length];
                    // video.Read(buffer, 0, buffer.Length);

                    // This is limited to 4.2GB files, but VI doesn't allow for byte array uploads bigger than 2GB.
                    byte[] videodata = File.ReadAllBytes(videoUrl?.LocalPath);

                    content.Add(new ByteArrayContent(videodata));
                    break;

                case UploadMethod.PublicUrl:
                    queryParams += $"&videoUrl={videoUrl}";
                    break;

                case UploadMethod.SendToBlobStorage:
                    break;

                default:
                    break;
            }

            Uri requestUri = new Uri($"{VideoIndexerApiUrl}/{this.accountLocation}/Accounts/{this.accountId}/Videos?{queryParams}");
            var uploadRequestResult = await client.PostAsync(requestUri, content).ConfigureAwait(false);
            content.Dispose();
            return await uploadRequestResult.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Builds a query string from a dictionary of key/value pairs.
        /// </summary>
        /// <param name="parameters">Dictionary of strings containing the key/value pairs of parameters.</param>
        /// <returns>The assembled query string.</returns>
        private static string CreateQueryString(IDictionary<string, string> parameters)
        {
            var queryParameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in parameters)
            {
                queryParameters[parameter.Key] = parameter.Value;
            }

            return queryParameters.ToString();
        }
    }
}
