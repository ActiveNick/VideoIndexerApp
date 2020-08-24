# VideoIndexerApp
Sample C# app used to interact with the Azure Media Services Video Indexer API. You will need a Video Indexer (VI) account to use this project.

## Reference Links
* [Video Indexer Product Home Page](https://azure.microsoft.com/services/media-services/video-indexer/)
* [Video Indexer Documentation](https://docs.microsoft.com/en-us/azure/media-services/video-indexer/)
* [Video Indexer User Portal](https://www.videoindexer.ai/)
* [Video Indexer Developer Portal](https://api-portal.videoindexer.ai/)
* [Azure Blob Storage Product Home Page](https://azure.microsoft.com/services/storage/blobs/)
* [Azure Blob Storage Documentation](https://docs.microsoft.com/en-us/azure/storage/blobs/)

## Solution Contents
This solution includes the following projects:

* **VideoIndexerLibrary**: Reusable .NET Standard 2 DLL library that acts as a partial SDK/wrapper around the Video Indexer API.
* **VideoIndexerClient.DemoConsole**: .NET Core console app used to consume the VI library.
* **VideoIndexerClient.UWP**: Windows 10 UWP XAML app used to consume the VI library.

## Configuration Settings
Before using either the console app or the UWP client, make sure to properly initialize the file **appsettings.json** with your own Azure & VI credentials. A sample file is provided named **appsettings.example.json**. You will need the following:

* Video Indexer Account ID
* Video Indexer API Key
* Video Indexer Account Region (East US, East US 2, West US, etc.)

If you intend on uploading video files and/or video thumbnails to blob storage, you will also need the following:

* Azure Blob Storage Account Name
* Azure Blob Storage Connection String

## How to Use the Console App
Launch the console app from the command line:

```VideoIndexerClient.DemoConsole.exe [arguments]```

The following arguments are supported:

```
[-downloadthumbnails | -dt value]   value: video id
[-upload | -up value]               value: path and file name of video to upload
[-name value]                       value: name of video to upload
[-description | -desc value]        value: description of video
[-savetofolder value]               value: local path where to save extracted thumbnails
[-savetocloud value]                value: Blob storage container prefix to save thumbnails
```

## Video Indexer Library Features
The Video Indexer Library project included in this repository is a partial wrapper around the Video Indexer API. The following features are currently supported:

* Obtain an account access token
* Retrieve all videos stored in an account
* Retrieve a specific thumbnail for a given video
* Retrieve the video indexer url for a given video
* Retrieve all the insights indexed by VI for a given video
* Upload a video to VI from a local file or a public url

## Implementation Notes

* Solution developed in Visual Studio 2019.
* All projects in this solution app make use of FxCop and StyleCop for coding standards.
* The account access token retrieval currently does not support automatic token refresh if it expires.
* Local file upload mode doesn't support multipart uploads yet and is currently currently limited to videos no larger than 2GB in size.
* There is currently an issue with file access permissions in the UWP client. UWP apps cannot access the full file system but we need to be able to select video files anywhere for upload. The broadFileSystemAccess capability doesn't seem to be picked up, or requires the use of Windows.Storage.StorageFile. But we cannot use this Windows-specific class in the .NET Standard library since it's meant to be cross-platform .NET code. Access to the video upload in the UWP app is disabled in the meantime until fixed. Please use the console app instead for now to upload videos.
* There is an intermitent issue with some videos where the insights data cannot be deserialized properly. Still investigating.
* Everything is a work in progress, to be used "as is" and is subject to change.
* Please file issues here in GitHub if you have questions or problems.

## License

All VideoIndexerApp documentation and samples included in this repository are licensed with the MIT License. For more details, see LICENSE.