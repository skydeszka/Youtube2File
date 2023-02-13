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
    /// The <see cref="VideoId"/> of the Youtube video.
    /// </summary>
    public required VideoId Id { get; init; }
    /// <summary>
    /// The Youtube client.
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
    public async ValueTask<IStreamInfo> GetPreferredAudioOrHighestBitrate(params Container[] containers)
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

    /// <summary>
    /// Retrieves the <see cref="Video"/> from Youtube and caches it.
    /// </summary>
    /// <returns>The retrieved <see cref="Video"/>.</returns>
    private async ValueTask<Video> GetVideo()
    {
        _video ??= await _client.Videos.GetAsync(Id);

        return _video;
    }

    /// <summary>
    /// Retrieves the <see cref="StreamManifest"/> from Youtube and caches it.
    /// </summary>
    /// <returns>The retrieved <see cref="StreamManifest"/>.</returns>
    private async ValueTask<StreamManifest> GetManifest()
    {
        _streamManifest ??= await _client.Videos.Streams.GetManifestAsync(Id);

        return _streamManifest;
    }
}
