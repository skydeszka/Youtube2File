﻿using System.Linq;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Youtube2File;

/// <summary>
/// Acts as an abstraction to the <see cref="YoutubeClient"/> class.
/// </summary>
public sealed class YoutubeDownloader
{
    [Flags]
    public enum DownloadType
    {
        Audio = 1,
        Video = 2,
        Caption = 4
    }

    public struct DownloadOptions
    {
        public required DownloadType Type { get; init; }
        public required Container PreferedContainer { get; init; }
    }
    /// <summary>
    /// The folder where the data will be saved at.
    /// </summary>

    private Video? _video;
    private StreamManifest? _streamManifest;

    public YoutubeDownloader()
    {
        _client = new();
    }

    {








        string extension = streamInfo.Container.ToString();

    }

    private async ValueTask DownloadVideo()
    {

    }
}
