using System.Linq;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Youtube2File;

/// <summary>
/// Acts as an abstraction to the <see cref="YoutubeClient"/> class.
/// </summary>
public sealed class YoutubeDownloader
{
    /// <summary>
    /// The folder where the data will be saved at.
    /// </summary>
    private readonly YoutubeClient _client;

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
