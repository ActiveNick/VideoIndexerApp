// <copyright file="VideoIndexerSearchResults.cs" company="Microsoft Corp">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>

/// <summary>
/// This file contains deserialization classes required to hold data related to Video Indexer API objects.
/// </summary>
namespace VideoIndexerLibrary
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Enum used by video upload to determine how the video files are to be sent to the cloud.
    /// </summary>
    public enum UploadMethod
    {
        /// <summary>
        /// The video uri points to a local file to be read and uploaded via the multipart form post API.
        /// </summary>
        LocalFile,

        /// <summary>
        /// The video uri points to a public file to be pulled directly by Video Indexer.
        /// </summary>
        PublicUrl,

        /// <summary>
        /// The video uri points to a local file that should first be uploaded to blob storage and then sent to VI.
        /// </summary>
        SendToBlobStorage,
    }

    /// <summary>
    /// Deserialization class which describes the metadata for Video Indexer search results obtained by the List Videos API.
    /// API doc: https://api-portal.videoindexer.ai/docs/services/Operations/operations/List-Videos.
    /// </summary>
    public class VideoIndexerSearchResults
    {
        /// <summary>
        /// Gets the list of videos resulting from a search.
        /// </summary>
        [JsonProperty("results")]
        public List<Video> Videos { get; } = new List<Video>();

        /// <summary>
        /// Gets or sets the next page of results.
        /// </summary>
        [JsonProperty("nextPage")]
        public Nextpage NextPage { get; set; }
    }

    /// <summary>
    /// Class used for paging through Video Indexer video search results.
    /// </summary>
    public class Nextpage
    {
        /// <summary>
        /// Gets or sets the number of results returned at a time in a video search.
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the number of pages skipped before this page of results.
        /// </summary>
        [JsonProperty("skip")]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the last page (true) or not.
        /// </summary>
        [JsonProperty("done")]
        public bool Done { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes the metadata for the Video content returned by the Get Video Index API.
    /// API doc: https://api-portal.videoindexer.ai/docs/services/Operations/operations/Get-Video-Index.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Gets or sets the video's VI account ID.
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the video's ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the partition (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("partition")]
        public object Partition { get; set; }

        /// <summary>
        /// Gets or sets the video's external ID (if specified by the user).
        /// </summary>
        [JsonProperty("externalId")]
        public object ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the video's external ID (if specified by the user).
        /// </summary>
        [JsonProperty("")]
        public object ExternalUrl { get; set; }

        /// <summary>
        /// Gets or sets the video's external metadata (if specified by the user).
        /// </summary>
        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        /// <summary>
        /// Gets or sets the video's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the video's description.
        /// </summary>
        [JsonProperty("description")]
        public object Description { get; set; }

        /// <summary>
        /// Gets or sets the video's creation date & time.
        /// </summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the video's date & time when it was last modified.
        /// </summary>
        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the video's date & time when it was last indexed in VI.
        /// </summary>
        [JsonProperty("lastIndexed")]
        public DateTime LastIndexed { get; set; }

        /// <summary>
        /// Gets or sets the video’s privacy mode (Private/Public).
        /// </summary>
        [JsonProperty("privacyMode")]
        public string PrivacyMode { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who created the video.
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the video was created by the current user.
        /// </summary>
        [JsonProperty("isOwned")]
        public bool IsOwned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the video is a base video or if it's derived from another video.
        /// </summary>
        [JsonProperty("isBase")]
        public bool IsBase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the video contains a source video file (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("hasSourceVideoFile")]
        public bool HasSourceVideoFile { get; set; }

        /// <summary>
        /// Gets or sets the video’s state (uploaded, processing, processed, failed, quarantined).
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the video's moderation state (TODO: Needs clarification on possible values).
        /// </summary>
        [JsonProperty("moderationState")]
        public string ModerationState { get; set; }

        /// <summary>
        /// Gets or sets the video's review state (TODO: Needs clarification on possible values).
        /// </summary>
        [JsonProperty("reviewState")]
        public string ReviewState { get; set; }

        /// <summary>
        /// Gets or sets the video's processing progress while being indexed by VI (for example, 20%).
        /// </summary>
        [JsonProperty("processingProgress")]
        public string ProcessingProgress { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the video.
        /// </summary>
        [JsonProperty("durationInSeconds")]
        public int DurationInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the ID of the video from which the thumbnail was taken.
        /// </summary>
        [JsonProperty("thumbnailVideoId")]
        public string ThumbnailVideoId { get; set; }

        /// <summary>
        /// Gets or sets the video's thumbnail ID.
        /// To get the actual thumbnail, call the Get-Thumbnail API and pass it thumbnailVideoId and thumbnailId.
        /// Reference doc: https://api-portal.videoindexer.ai/docs/services/Operations/operations/Get-Video-Thumbnail.
        /// </summary>
        [JsonProperty("thumbnailId")]
        public string ThumbnailId { get; set; }

        /// <summary>
        /// Gets the search matches (TODO: Needs clarification).
        /// The structure of this object is still unknown since it was empty in the JSON results returned so far.
        /// TODO: Change the type 'object' to a specific type.
        /// </summary>
        [JsonProperty("searchMatches")]
        public List<object> SearchMatches { get; } = new List<object>();

        /// <summary>
        /// Gets or sets the preset used to index the video.
        /// </summary>
        [JsonProperty("indexingPreset")]
        public string IndexingPreset { get; set; }

        /// <summary>
        /// Gets or sets the preset used to publish the video.
        /// </summary>
        [JsonProperty("streamingPreset")]
        public string StreamingPreset { get; set; }

        /// <summary>
        /// Gets or sets the video's source language (assuming one master language). In the form of a BCP-47 string.
        /// Reference link: https://tools.ietf.org/html/bcp47.
        /// </summary>
        [JsonProperty("sourceLanguage")]
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Gets the video's source languages (TODO: Need to clarify if this is a list of all language in a multi-track video).
        /// </summary>
        [JsonProperty("sourceLanguages")]
        public List<string> SourceLanguages { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the video's actual language (translation).
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets the video's actual languages (TODO: Need to clarify if this is a list of all language in a multi-track video).
        /// </summary>
        [JsonProperty("languages")]
        public List<string> Languages { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the source language was detected (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("detectSourceLanguage")]
        public bool DetectSourceLanguage { get; set; }

        /// <summary>
        /// Gets or sets the language autodetect mode for the video (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("languageAutoDetectMode")]
        public string LanguageAutoDetectMode { get; set; }

        /// <summary>
        /// Gets or sets the CRIS (Custom Recognition Intelligent Service) model used to transcribe the video.
        /// Docs: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/multi-language-identification-transcription.
        /// </summary>
        [JsonProperty("linguisticModelId")]
        public string LinguisticModelId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the person model used to index the video.
        /// </summary>
        [JsonProperty("personModelId")]
        public string PersonModelId { get; set; }

        /// <summary>
        /// Gets or sets the failure code if failed to process (for example, 'UnsupportedFileType').
        /// </summary>
        [JsonProperty("failureCode")]
        public string FailureCode { get; set; }

        /// <summary>
        /// Gets or sets the failure message if failed to process.
        /// </summary>
        [JsonProperty("failureMessage")]
        public string FailureMessage { get; set; }

        /// <summary>
        /// Gets or sets the insights associated with the video.
        /// </summary>
        [JsonProperty("insights")]
        public Insights Insights { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the video was manually reviewed and identified as an adult video.
        /// </summary>
        [JsonProperty("isAdult")]
        public bool IsAdult { get; set; }

        /// <summary>
        /// Gets or sets a url to stream the video.
        /// </summary>
        [JsonProperty("publishedUrl")]
        public System.Uri PublishedUrl { get; set; }

        /// <summary>
        /// Gets or sets a url to stream the video from (for Apple devices).
        /// </summary>
        [JsonProperty("publishedProxyUrl")]
        public object PublishedProxyUrl { get; set; }

        /// <summary>
        /// Gets or sets a short lived view token for streaming the video.
        /// </summary>
        [JsonProperty("viewToken")]
        public string ViewToken { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes the insights metadata produced by VI for a given video.
    /// Each insight (for example, transcript lines, faces, brands, etc.), contains a list of unique elements (for example, face1, face2, face3),
    /// and each element has its own metadata and a list of its instances (which are time ranges with additional optional metadata).
    /// A face might have an ID, a name, a thumbnail, other metadata, and a list of its temporal instances
    /// (for example: 00:00:05 – 00:00:10, 00:01:00 - 00:02:30 and 00:41:21 – 00:41:49.)
    /// Each temporal instance can have additional metadata.For example, the face’s rectangle coordinates(20,230,60,60).
    /// </summary>
    public class Insights
    {
        /// <summary>
        /// Gets or sets the version (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the version (TODO: Needs clarification on what this is associated with).
        /// </summary>
        [JsonProperty("duration")]
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the video's source language (assuming one master language). In the form of a BCP-47 string.
        /// Reference link: https://tools.ietf.org/html/bcp47.
        /// </summary>
        [JsonProperty("sourceLanguage")]
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Gets the video's source languages (TODO: Need to clarify if this is a list of all language in a multi-track video).
        /// </summary>
        [JsonProperty("sourceLanguages")]
        public List<string> SourceLanguages { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the video's actual language (translation).
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets the video's actual languages (TODO: Need to clarify if this is a list of all language in a multi-track video).
        /// </summary>
        [JsonProperty("languages")]
        public List<string> Languages { get; } = new List<string>();

        /// <summary>
        /// Gets the transcript insight.
        /// </summary>
        [JsonProperty("transcript")]
        public List<Transcript> Transcript { get; } = new List<Transcript>();

        /// <summary>
        /// Gets the Ocr insight.
        /// </summary>
        [JsonProperty("ocr")]
        public List<Ocr> Ocr { get; } = new List<Ocr>();

        /// <summary>
        /// Gets the Keywords insight.
        /// </summary>
        [JsonProperty("keywords")]
        public List<Keyword> Keywords { get; } = new List<Keyword>();

        /// <summary>
        /// Gets the Topics insight.
        /// </summary>
        [JsonProperty("topics")]
        public List<Topic> Topics { get; } = new List<Topic>();

        /// <summary>
        /// Gets the Faces insight.
        /// </summary>
        [JsonProperty("faces")]
        public List<Face> Faces { get; } = new List<Face>();

        /// <summary>
        /// Gets the Labels insight.
        /// </summary>
        [JsonProperty("labels")]
        public List<Label> Labels { get; } = new List<Label>();

        /// <summary>
        /// Gets the Scenes insight.
        /// </summary>
        [JsonProperty("scenes")]
        public List<Scene> Scenes { get; } = new List<Scene>();

        /// <summary>
        /// Gets the Shots insight.
        /// </summary>
        [JsonProperty("shots")]
        public List<Shot> Shots { get; } = new List<Shot>();

        /// <summary>
        /// Gets the Brands insight.
        /// </summary>
        [JsonProperty("brands")]
        public List<Brand> Brands { get; } = new List<Brand>();

        /// <summary>
        /// Gets the NamedLocations insight.
        /// </summary>
        [JsonProperty("namedLocations")]
        public List<Namedentity> NamedLocations { get; } = new List<Namedentity>();

        /// <summary>
        /// Gets the NamedPeople insight.
        /// </summary>
        [JsonProperty("namedPeople")]
        public List<Namedentity> NamedPeople { get; } = new List<Namedentity>();

        /// <summary>
        /// Gets the Sentiments insight.
        /// </summary>
        [JsonProperty("sentiments")]
        public List<Sentiment> Sentiments { get; } = new List<Sentiment>();

        /// <summary>
        /// Gets the Emotions insight.
        /// </summary>
        [JsonProperty("emotions")]
        public List<Emotion> Emotions { get; } = new List<Emotion>();

        /// <summary>
        /// Gets the VisualContentModeration insight.
        /// </summary>
        [JsonProperty("visualContentModeration")]
        public List<Visualcontentmoderation> VisualContentModeration { get; } = new List<Visualcontentmoderation>();

        /// <summary>
        /// Gets one or more blocks (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("blocks")]
        public List<Block> Blocks { get; } = new List<Block>();

        /// <summary>
        /// Gets the Frame Patterns (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("framePatterns")]
        public List<Framepattern> FramePatterns { get; } = new List<Framepattern>();

        /// <summary>
        /// Gets the Speakers insight.
        /// </summary>
        [JsonProperty("speakers")]
        public List<Speaker> Speakers { get; } = new List<Speaker>();

        /// <summary>
        /// Gets or sets the TextualContentModeration (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("textualContentModeration")]
        public Textualcontentmoderation TextualContentModeration { get; set; }

        /// <summary>
        /// Gets or sets the Speaker statistics.
        /// </summary>
        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes the metadata for Video Indexer search results obtained by the Get Video Index API.
    /// API doc: https://api-portal.videoindexer.ai/docs/services/Operations/operations/Get-Video-Index.
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Gets or sets the partition (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("partition")]
        public object Partition { get; set; }

        /// <summary>
        /// Gets or sets the playlist's description.
        /// </summary>
        [JsonProperty("description")]
        public object Description { get; set; }

        /// <summary>
        /// Gets or sets the playlist’s privacy mode (Private/Public).
        /// </summary>
        [JsonProperty("privacyMode")]
        public string PrivacyMode { get; set; }

        /// <summary>
        /// Gets or sets the playlist’s (uploaded, processing, processed, failed, quarantined).
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the playlist's VI account ID.
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the playlist's ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the playlist's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// gets or sets the name of the user who created the playlist.
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the playlist's creation date & time.
        /// </summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the playlist was created by the current user.
        /// </summary>
        [JsonProperty("isOwned")]
        public bool IsOwned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is authorized to edit the playlist.
        /// </summary>
        [JsonProperty("isEditable")]
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the playlist is a base playlist (a video) or a playlist made of other videos (derived).
        /// </summary>
        [JsonProperty("isBase")]
        public bool IsBase { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the playlist.
        /// </summary>
        [JsonProperty("durationInSeconds")]
        public int DurationInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the Summaried Insights.
        /// </summary>
        [JsonProperty("summarizedInsights")]
        public Summarizedinsights SummarizedInsights { get; set; }

        /// <summary>
        /// Gets a list of videos constructing the playlist.
        /// If this playlist of constructed of time ranges of other videos (derived),
        /// the videos in this list will contain only data from the included time ranges.
        /// </summary>
        [JsonProperty("videos")]
        public List<Video> Videos { get; } = new List<Video>();

        /// <summary>
        /// Gets a list of ranges (start/end timestamps) included in this playlist for each video *by id).
        /// </summary>
        [JsonProperty("videosRanges")]
        public List<Videosrange> VideosRanges { get; } = new List<Videosrange>();
    }

    /// <summary>
    /// Deserialization class which contains the metadata summary of indexed insights in a video.
    /// Doc: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#summarizedinsights.
    /// </summary>
    public class Summarizedinsights
    {
        /// <summary>
        /// Gets or sets the name of the video. For example, Build 2019 Keynote.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the video. For example, 63c6d532ff.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the privacy setting of the video, which can have one of the following modes: Private, Public.
        /// Public - the video is visible to everyone in your account and anyone that has a link to the video.
        /// Private - the video is visible to everyone in your account.
        /// </summary>
        [JsonProperty("privacyMode")]
        public string PrivacyMode { get; set; }

        /// <summary>
        /// Gets or sets one duration that describes the time an insight occurred. Duration is in seconds.
        /// </summary>
        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        /// <summary>
        /// Gets or sets the ID of the video from which the thumbnail was taken.
        /// </summary>
        [JsonProperty("thumbnailVideoId")]
        public string ThumbnailVideoId { get; set; }

        /// <summary>
        /// gets or sets the video's thumbnail ID.
        /// To get the actual thumbnail, call Get-Thumbnail and pass it thumbnailVideoId and thumbnailId.
        /// Reference doc: https://api-portal.videoindexer.ai/docs/services/Operations/operations/Get-Video-Thumbnail.
        /// </summary>
        [JsonProperty("thumbnailId")]
        public string ThumbnailId { get; set; }

        /// <summary>
        /// Gets the list of audio effects contained in the video. For example, Clapping.
        /// May contain zero or more audio effects. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#audioeffects.
        /// </summary>
        [JsonProperty("audioEffects")]
        public List<Audioeffects> AudioEffects { get; } = new List<Audioeffects>();

        /// <summary>
        /// Gets the list of faces contained in the video.
        /// May contain zero or more faces. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#faces.
        /// </summary>
        [JsonProperty("faces")]
        public List<Face> Faces { get; } = new List<Face>();

        /// <summary>
        /// Gets the list of keywords contained in the video. For example, technology, plan, sales receipts, tax guy, etc.
        /// May contain zero or more keywords. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#keywords.
        /// </summary>
        [JsonProperty("keywords")]
        public List<Keyword> Keywords { get; } = new List<Keyword>();

        /// <summary>
        /// Gets the list of sentiments contained in the video. For example, Neutral, Positive, Negative.
        /// May contain zero or more sentiments. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#sentiments.
        /// </summary>
        [JsonProperty("sentiments")]
        public List<Sentiment> Sentiments { get; } = new List<Sentiment>();

        /// <summary>
        /// Gets the list of emotions contained in the video. For example, Joy, Fear, Anger, Sad, etc.
        /// May contain zero or more emotions. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#emotions.
        /// </summary>
        [JsonProperty("emotions")]
        public List<Emotion> Emotions { get; } = new List<Emotion>();

        /// <summary>
        /// Gets the list of labels contained in the video. For example, person, indoor, outdoor, etc.
        /// May contain zero or more labels. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#labels.
        /// </summary>
        [JsonProperty("labels")]
        public List<Label> Labels { get; } = new List<Label>();

        /// <summary>
        /// Gets the frame patterns in the video. For example, Black, Rolling Credits, etc.
        /// </summary>
        [JsonProperty("framePatterns")]
        public List<Framepattern> FramePatterns { get; } = new List<Framepattern>();

        /// <summary>
        /// Gets the list of brands contained in the video. For example, Microsoft, Canwest, Oldsmobile_Cutlass, etc.
        /// May contain zero or more brands. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#brands.
        /// </summary>
        [JsonProperty("brands")]
        public List<Brand> Brands { get; } = new List<Brand>();

        /// <summary>
        /// Gets the list of named locations contained in the video. For example, Canada, Saskatchewan, New Jersey, New York City, etc.
        /// May contain zero or more named locations.
        /// </summary>
        [JsonProperty("namedLocations")]
        public List<Namedentity> NamedLocations { get; } = new List<Namedentity>();

        /// <summary>
        /// Gets the list of named people contained in the video. For example, Brent_Butt, Tommy_Douglas, Al_Pacino, George_Clooney, Marlon_Brando, etc.
        /// May contain zero or more named people.
        /// </summary>
        [JsonProperty("namedPeople")]
        public List<Namedentity> NamedPeople { get; } = new List<Namedentity>();

        /// <summary>
        /// Gets or sets the correspondence and speaker statistics in the video. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#statistics.
        /// </summary>
        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }

        /// <summary>
        /// Gets the list of faces contained in the video. For example, Politics and Government, Law and Justice/Justice/Criminal Justice, etc.
        /// May contain zero or more faces. For more info, see link:
        /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#topics.
        /// </summary>
        [JsonProperty("topics")]
        public List<Topic> Topics { get; } = new List<Topic>();
    }

    /// <summary>
    /// Deserialization class which contains metadata for a duration.
    /// </summary>
    public class Duration
    {
        /// <summary>
        /// Gets or sets a time.
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets a duration in seconds.
        /// </summary>
        [JsonProperty("seconds")]
        public float Seconds { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes metadata for an appearance of an element appeared in the given time range.
    /// This can be a Face, Emotion, Brand, Sentiment, Label, etc.
    /// </summary>
    public class Appearance
    {
        /// <summary>
        /// Gets or sets the start time of an appearance.
        /// </summary>
        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of an appearance.
        /// </summary>
        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets the start of an appearance as expressed in seconds since the beginning of the video.
        /// </summary>
        [JsonProperty("startSeconds")]
        public float StartSeconds { get; set; }

        /// <summary>
        /// Gets or sets the end of an appearance as expressed in seconds since the beginning of the video.
        /// </summary>
        [JsonProperty("endSeconds")]
        public float EndSeconds { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes metadata for an instance of an element appeared in the given time range.
    /// This can be a Face, Emotion, Brand, Sentiment, Label, etc.
    /// </summary>
    public class Instance
    {
        /// <summary>
        /// Gets or sets the ID of the thumbnail associated with this instance (only used for instances of keyframes).
        /// </summary>
        [JsonProperty("thumbnailId")]
        public string ThumbnailId { get; set; }

        /// <summary>
        /// Gets or sets the brand type (only used for brand insight instances).
        /// For example, Transcript, Ocr.
        /// </summary>
        [JsonProperty("brandType")]
        public string BrandType { get; set; }

        /// <summary>
        /// Gets or sets the brand's instanceSource (only used for insight instances of brand, named locations).
        /// For example, Transcript, Ocr.
        /// </summary>
        [JsonProperty("instanceSource")]
        public string InstanceSource { get; set; }

        /// <summary>
        /// Gets or sets the adjusted start time of an instance (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("adjustedStart")]
        public string AdjustedStart { get; set; }

        /// <summary>
        /// Gets or sets the adjusted end time of an instance (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("adjustedEnd")]
        public string AdjustedEnd { get; set; }

        /// <summary>
        /// Gets or sets the start time of an instance.
        /// </summary>
        [JsonProperty("start")]
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the end time of an instance.
        /// </summary>
        [JsonProperty("end")]
        public string End { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes the metadata for the correspondence and speaker statistics in the video.
    /// For more info, see link:
    /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#statistics.
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Gets or sets the number of correspondences in the video.
        /// </summary>
        [JsonProperty("correspondenceCount")]
        public int CorrespondenceCount { get; set; }

        /// <summary>
        /// Gets the number of words per speaker.
        /// </summary>
        [JsonProperty("speakerTalkToListenRatio")]
        public Dictionary<string, float> SpeakerTalkToListenRatio { get; } = new Dictionary<string, float>();

        /// <summary>
        /// Gets the amount of fragments the speaker has in a video.
        /// </summary>
        [JsonProperty("speakerLongestMonolog")]
        public Dictionary<string, int> SpeakerLongestMonolog { get; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets the speaker's longest monolog. If the speaker has silences inside the monolog it is included.
        /// Silence at the beginning and the end of the monolog is removed.
        /// </summary>
        [JsonProperty("speakerNumberOfFragments")]
        public Dictionary<string, int> SpeakerNumberOfFragments { get; } = new Dictionary<string, int>();

        /// <summary>
        /// Gets the word count the speaker in a video.
        /// The calculation is based on the time spent on the speaker's monolog (without the silence in between) divided by the total time of the video.
        /// The time is rounded to the third decimal point.
        /// </summary>
        [JsonProperty("speakerWordCount")]
        public Dictionary<string, int> SpeakerWordCount { get; } = new Dictionary<string, int>();
    }

    /// <summary>
    /// Deserialization class which describes the metadata for the list of audio effects contained in the video. For example, Clapping, Speech, Silence.
    /// May contain zero or more audio effects. For more info, see link:
    /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#audioeffects.
    /// </summary>
    public class Audioeffects
    {
        /// <summary>
        /// Gets or sets the audio effect ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the audio effect type (for example, Clapping, Speech, Silence).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets a list of time ranges where this audio effect appeared.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the metadata for the list of faces contained in the video.
    /// May contain zero or more faces. For more info, see link:
    /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#faces.
    /// </summary>
    public class Face
    {
        /// <summary>
        /// Gets or sets the face ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// gets or sets rhe name of the face.
        /// It can be 'Unknown #0, an identified celebrity or a customer trained person.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets rhe video's ID.
        /// </summary>
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        /// <summary>
        /// Gets or sets the reference ID. If it is a Bing celebrity, its Bing ID.
        /// </summary>
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the reference type. Currently, just Bing.
        /// </summary>
        [JsonProperty("referenceType")]
        public string ReferenceType { get; set; }

        /// <summary>
        /// Gets or sets the face identification confidence (0-1). Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets a description of the celebrity.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the title of the celebrity.
        /// If it is a celebrity, its title (for example "Microsoft's CEO").
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the known person ID of the celebrity.
        /// If it is a known person, its internal ID.
        /// </summary>
        [JsonProperty("knownPersonId")]
        public string KnownPersonId { get; set; }

        /// <summary>
        /// Gets or sets the image Url of the celebrity.
        /// If it is a celebrity, its image url.
        /// </summary>
        [JsonProperty("imageUrl")]
        public System.Uri ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the ID of the thumbnail of that face.
        /// </summary>
        [JsonProperty("thumbnailId")]
        public string ThumbnailId { get; set; }

        /// <summary>
        /// Gets or sets the duration that indicates how long a celebrity is seen in a video (in seconds).
        /// </summary>
        [JsonProperty("seenDuration")]
        public float SeenDuration { get; set; }

        /// <summary>
        /// Gets or sets the screen time ratio that indicates how long a celebrity is seen in a video (in seconds).
        /// It is calculated by dividing seenDuration by the total duration of the video.
        /// </summary>
        [JsonProperty("seenDurationRatio")]
        public float SeenDurationRatio { get; set; }

        /// <summary>
        /// Gets the list of appearances where this face appears in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets the list of instances where this face appears in the video (used in Video.Insights).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the metadata for noteworthy keywords detected by Video Indexer in a video.
    /// </summary>
    public class Keyword
    {
        /// <summary>
        /// Gets or sets a value indicating whether the keyword is in the transcript (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("isTranscript")]
        public bool IsTranscript { get; set; }

        /// <summary>
        /// Gets or sets the keyword ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text of the keyword (used in Summarized Insights).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the text of the keyword (used in Video.Insights).
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the keyword's recognition confidence (0-1). Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the keyword language (when translated), in the form of a BCP-47 string.
        /// Reference: https://tools.ietf.org/html/bcp47.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets the list of appearances where this keyword appears or is mentioned in a video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets the list of instances where this keyword appears or is mentioned in a video (used in Video.Insights).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the sentiment metadata detected in a video.
    /// Sentiments are aggregated by their sentimentType field (Positive/Neutral/Negative).
    /// For example, 0-0.1, 0.1-0.2.
    /// </summary>
    public class Sentiment
    {
        /// <summary>
        /// Gets or sets the sentiment's ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sentiment type (used in SummarizedInsights).
        /// The type can be 'Positive', 'Neutral', or 'Negative'.
        /// </summary>
        [JsonProperty("sentimentKey")]
        public string SentimentKey { get; set; }

        /// <summary>
        /// Gets or sets the sentiment type (used in Video.Insights).
        /// </summary>
        [JsonProperty("sentimentType")]
        public string SentimentType { get; set; }

        /// <summary>
        /// Gets or sets the screen time ratio that indicates how long a sentiment is detected in a video (used in SummarizedInsights).
        /// It is calculated by dividing the sum of all instances durations for that sentiment by the total duration of the video.
        /// </summary>
        [JsonProperty("seenDurationRatio")]
        public float SeenDurationRatio { get; set; }

        /// <summary>
        /// Gets or sets the average of all scores of all instances of that sentiment type - Positive/Neutral/Negative (used in Video.Insights).
        /// </summary>
        [JsonProperty("averageScore")]
        public float AverageScore { get; set; }

        /// <summary>
        /// Gets the list of appearances where this sentiment is detected in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets the list of instances where this sentiment is detected in the video (used in Video.Insights).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the emotion metadata detected in a video.
    /// Video Indexer identifies emotions based on speech and audio cues.
    /// The identified emotion could be: joy, sadness, anger, or fear.
    /// </summary>
    public class Emotion
    {
        /// <summary>
        /// Gets or sets the emotion's ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the emotion moment that was identified based on speech and audio cues.
        /// The emotion could be: joy, sadness, anger, or fear.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the screen time ratio that indicates how long an emotion is detected in a video (used in SummarizedInsights).
        /// It is calculated by dividing the sum of all instances durations for that emotion by the total duration of the video.
        /// </summary>
        [JsonProperty("seenDurationRatio")]
        public float SeenDurationRatio { get; set; }

        /// <summary>
        /// Gets the list of appearances where this emotion is detected in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets the list of instances where this emotion is detected in the video (used in Video.Insights).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the labels metadata detected in the video. For example, person, indoor, outdoor, etc.
    /// May contain zero or more labels. For more info, see link:
    /// https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#labels.
    /// </summary>
    public class Label
    {
        /// <summary>
        /// Gets or sets the label ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the label name (for example, 'Computer', 'TV').
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the label name language (when translated), in the form of a BCP-47 string.
        /// Reference: https://tools.ietf.org/html/bcp47.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets the list of appearances where this label is detected in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets the list of instances where this label is detected in the video (used in Video.Insights).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the frame patterns metadata in the video.
    /// For example, Black, Rolling Credits, etc.
    /// </summary>
    public class Framepattern
    {
        /// <summary>
        /// Gets or sets the id of the frame pattern.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the frame pattern.
        /// For example, Black, Rolling Credits, etc.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the frame pattern's recognition confidence (0-1). Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets the list of appearances where this frame pattern is detected in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets the list of instances where this frame pattern is detected in the video (used in Video.Insights).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the Brands insight metadata detected in the video.
    /// Brands include business and product brand names detected in the speech to text transcript and/or Video OCR.
    /// This does not include visual recognition of brands or logo detection.
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// Gets or sets the brand ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the brand's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the suffix of the brand's wikipedia url.
        /// For example, "Target_Corporation” is the suffix of https://en.wikipedia.org/wiki/Target_Corporation.
        /// </summary>
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the source of the reference. For example, Wiki.
        /// </summary>
        [JsonProperty("referenceType")]
        public string ReferenceType { get; set; }

        /// <summary>
        /// Gets or sets the brand's Wikipedia url, if it exists.
        /// For example, https://en.wikipedia.org/wiki/Target_Corporation.
        /// </summary>
        [JsonProperty("referenceUrl")]
        public System.Uri ReferenceUrl { get; set; }

        /// <summary>
        /// Gets or sets the brand's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets a list of predefined tags that were associated with this brand.
        /// </summary>
        [JsonProperty("tags")]
        public List<string> Tags { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the confidence value of the Video Indexer brand detector (0-1). Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the duration that indicates how long a brand is seen in a video (in seconds).
        /// </summary>
        [JsonProperty("seenDuration")]
        public float SeenDuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the detected brand is a custom brand (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("isCustom")]
        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets a list of time ranges of this brand (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets a list of time ranges for this brand (used in Video.Insights).
        /// Each instance has a brandType, which indicates whether this brand appeared in the transcript or in OCR.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes metadata for named people and locations detected in the video.
    /// Location examples: Canada, Saskatchewan, Montreal, New Jersey, New York City, etc.
    /// People examples: Tommy Douglas, Al Pacino, George Clooney, Marlon Brando, Brent Butt, Eliot Ness, etc.
    /// </summary>
    public class Namedentity
    {
        /// <summary>
        /// Gets or sets the named people or location ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of a person or location.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the suffix of the named person or location's wikipedia url.
        /// For example, "Canada” is the suffix of https://en.wikipedia.org/wiki/Canada.
        /// </summary>
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the named person or location's Wikipedia url, if it exists.
        /// For example, https://en.wikipedia.org/wiki/Oklahoma.
        /// </summary>
        [JsonProperty("referenceUrl")]
        public System.Uri ReferenceUrl { get; set; }

        /// <summary>
        /// Gets or sets the confidence value of the Video Indexer named people/location detector (0-1). Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the named person or location's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the duration that indicates how long a named person or location is seen in a video (in seconds).
        /// </summary>
        [JsonProperty("seenDuration")]
        public float SeenDuration { get; set; }

        /// <summary>
        /// Gets a list of predefined tags that were associated with this named person or location.
        /// </summary>
        [JsonProperty("tags")]
        public List<string> Tags { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the detected person or location is a custom location (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("isCustom")]
        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets the list of appearances where this named person or location is detected in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets a list of time ranges for this named person or location (used in Video.Insights).
        /// Each instance has an instanceSource, which indicates whether this named person or location appeared in the transcript or in OCR.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes main topics inferred by Video Indexer from transcripts.
    /// When possible, the 2nd-level IPTC taxonomy is included.
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// Gets or sets the topic ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the topic name, for example: "Pharmaceuticals".
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets breadcrumbs reflecting the topics hierarchy.
        /// For example: "Health and wellbeing / Medicine and healthcare / Pharmaceuticals".
        /// </summary>
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the source of the reference. For example, VideoIndexer.
        /// </summary>
        [JsonProperty("referenceType")]
        public string ReferenceType { get; set; }

        /// <summary>
        /// Gets or sets the reference url.
        /// </summary>
        [JsonProperty("referenceUrl")]
        public System.Uri ReferenceUrl { get; set; }

        /// <summary>
        /// Gets or sets the IPTC media code name, if detected. For example, Politics.
        /// </summary>
        [JsonProperty("iptcName")]
        public string IptcName { get; set; }

        /// <summary>
        /// Gets or sets the IAB name (TODO: Needs clarification).
        /// </summary>
        [JsonProperty("iabName")]
        public string IabName { get; set; }

        /// <summary>
        /// Gets or sets the confidence score in the range [0,1]. Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the language used in the topic, in the form of a BCP-47 string.
        /// Reference: https://tools.ietf.org/html/bcp47.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets the list of appearances where this topic is detected in the video (used in SummarizedInsights).
        /// </summary>
        [JsonProperty("appearances")]
        public List<Appearance> Appearances { get; } = new List<Appearance>();

        /// <summary>
        /// Gets a list of time ranges for this topic (used in Video.Insights).
        /// Currently, Video Indexer does not index a topic to time intervals, so the whole video is used as the interval.
        /// TODO: This statement needs clarification since actual instances are being reported by the indexer.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes content moderation related to banned words detected in the text of the video.
    /// </summary>
    public class Textualcontentmoderation
    {
        /// <summary>
        /// Gets or sets the textual content moderation ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the number of banned words.
        /// </summary>
        [JsonProperty("bannedWordsCount")]
        public int BannedWordsCount { get; set; }

        /// <summary>
        /// Gets or sets the ratio from total number of words.
        /// </summary>
        [JsonProperty("bannedWordsRatio")]
        public float BannedWordsRatio { get; set; }

        /// <summary>
        /// Gets a list of time ranges for banned words in the video (used in Video.Insights).
        /// Each instance has an instanceSource, which indicates whether this named person or location appeared in the transcript or in OCR.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the full transcript of the video as detected by the Video Indexer Speech Recognizer.
    /// Each entry in the list is a single line in the transcript.
    /// Doc: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/multi-language-identification-transcription.
    /// Can only be accessed via Video.Insights.
    /// </summary>
    public class Transcript
    {
        /// <summary>
        /// Gets or sets the line ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the transcript itself.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the confidence value of the Video Indexer transcription for this line (0-1). Higher is more confident.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the ID of the speaker for this line in the transcript.
        /// </summary>
        [JsonProperty("speakerId")]
        public int SpeakerId { get; set; }

        /// <summary>
        /// Gets or sets the transcript language. Intended to support transcript where each line can have a different language.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets a list of time ranges where this line appeared. If the instance is transcript, it will have only 1 instance.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes text in the video that was detected via Optical Character Recognition (OCR).
    /// Can only be accessed via Video.Insights.
    /// </summary>
    public class Ocr
    {
        /// <summary>
        /// Gets or sets the OCR line ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the OCR text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the recognition confidence (0-1). Higher is better.
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets the left location of the OCR rectangle in pixels.
        /// </summary>
        [JsonProperty("left")]
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the top location of the OCR rectangle in pixels.
        /// </summary>
        [JsonProperty("top")]
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the width of the OCR rectangle.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the OCR rectangle.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the OCR language.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets a list of time ranges where this OCR appeared (the same OCR can appear multiple times).
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes scenes in the video.
    /// Doc: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/scenes-shots-keyframes.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Gets or sets the ID of the scene.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets a list of time ranges for this scene. If the instance is scene, it will have only 1 instance.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes shots in the video.
    /// A scene is comprised of one or more shots.
    /// Doc: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/scenes-shots-keyframes.
    /// </summary>
    public class Shot
    {
        /// <summary>
        /// Gets or sets the id of the shot.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets a list of keyframes for this shot.
        /// </summary>
        [JsonProperty("keyFrames")]
        public List<Keyframe> KeyFrames { get; } = new List<Keyframe>();

        /// <summary>
        /// Gets a list of time ranges for this shot. If the instance is shot, it will have only 1 instance.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes shots in the video.
    /// A shot is comprised of one or more keyframes.
    /// Doc: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/scenes-shots-keyframes.
    /// </summary>
    public class Keyframe
    {
        /// <summary>
        /// Gets or sets the ID of the keyframe.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets a list of time ranges for this keyframe. If the instance is keyframe, it will have only 1 instance.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes moderation elements detected in the video related to visual content.
    /// The visualContentModeration block contains time ranges which Video Indexer found to potentially have adult content.
    /// If visualContentModeration is empty, there is no adult content that was identified.
    /// Doc: https://docs.microsoft.com/en-us/azure/media-services/video-indexer/video-indexer-output-json-v2#visualcontentmoderation.
    /// </summary>
    public class Visualcontentmoderation
    {
        /// <summary>
        /// Gets or sets the visual content moderation ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the adult score associated with this moderated content detected (0-1). Higher means mature or X-rated adult content.
        /// </summary>
        [JsonProperty("adultScore")]
        public float AdultScore { get; set; }

        /// <summary>
        /// Gets or sets the racy score associated with this moderated content detected (0-1). Higher means more racy and less family friendly content.
        /// </summary>
        [JsonProperty("racyScore")]
        public float RacyScore { get; set; }

        /// <summary>
        /// Gets a list of time ranges where this visual content moderation appeared.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class used to describe blocks in the video (TODO: Needs clarification).
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Gets or sets the block ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets a list of time ranges where of this block.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class used to describe appearances of a speaker in a video.
    /// </summary>
    public class Speaker
    {
        /// <summary>
        /// Gets or sets the id of the speaker.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the speaker.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets a list of time ranges where this speaker appears.
        /// </summary>
        [JsonProperty("instances")]
        public List<Instance> Instances { get; } = new List<Instance>();
    }

    /// <summary>
    /// Deserialization class which describes the video's ranges (TODO: Needs clarification).
    /// </summary>
    public class Videosrange
    {
        /// <summary>
        /// Gets or sets the ID of the video.
        /// </summary>
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        [JsonProperty("range")]
        public Range Range { get; set; }
    }

    /// <summary>
    /// Deserialization class which describes a video range, expressed with a start and end time.
    /// </summary>
    public class Range
    {
        /// <summary>
        /// Gets or sets the start time of the range.
        /// </summary>
        [JsonProperty("start")]
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the end time of the range.
        /// </summary>
        [JsonProperty("end")]
        public string End { get; set; }
    }
}
