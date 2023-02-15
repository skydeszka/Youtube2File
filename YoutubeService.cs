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

    /// <summary>
    /// Gets the audio in the first found preferred <see cref="Container"/> type
    /// or if no match found gets the one with the highest bitrate.
    /// </summary>
    /// <param name="containers">The prefered containers.</param>
    /// <returns>The prefered or best quality <see cref="IStreamInfo"/>.</returns>
    private async ValueTask<IStreamInfo> GetPreferredAudioOrHighestBitrate(params Container[] containers)
    {
        var manifest = await GetManifest();

        var audioOnlyStreams = manifest.GetAudioOnlyStreams();

        IStreamInfo? stream = null;

        foreach (Container container in containers)
        {
            var matches = audioOnlyStreams
                .Where(stream => stream.Container == container);

            if (!matches.Any())
                continue;

            stream = matches.GetWithHighestBitrate();
        }

        stream ??= audioOnlyStreams.GetWithHighestBitrate();

        return stream;
    }

        string extension = streamInfo.Container.ToString();

    }

    private async ValueTask DownloadVideo()
    {

    }
}
