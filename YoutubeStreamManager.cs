using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Videos;
using YoutubeExplode;

namespace Youtube2File;

/// <summary>
/// Manages the streams retrieved from Youtube.
/// </summary>
internal sealed class YoutubeStreamManager
{
    private readonly YoutubeClient _client;
    private VideoId _id;
    private StreamManifest _streamManifest = null!;

    private YoutubeStreamManager(YoutubeClient client, VideoId id)
    {
        _client = client;
        _id = id;
    }

    /// <summary>
    /// Creates a <see cref="YoutubeStreamManager"/> class with the given <see cref="YoutubeClient"/> and <see cref="VideoId"/>.
    /// </summary>
    /// <param name="client">The client to use.</param>
    /// <param name="id">The ID of the video.</param>
    /// <returns>The stream manager of the video.</returns>
    public static async Task<YoutubeStreamManager> Create(YoutubeClient client, VideoId id)
    {
        YoutubeStreamManager manager = new(client, id);

        manager._streamManifest = await manager._client
            .Videos
            .Streams
            .GetManifestAsync(manager._id);

        return manager;
    }

    /// <summary>
    /// Gets the <see cref="VideoOnlyStreamInfo"/> of the video with the best quality.
    /// </summary>
    /// <returns>The <see cref="VideoOnlyStreamInfo"/> with the best quality</returns>
    public VideoOnlyStreamInfo GetBestVideoStream()
    {
        var videoOnlyStream = _streamManifest.GetVideoOnlyStreams()
            .GetWithHighestVideoQuality();

        return (VideoOnlyStreamInfo)videoOnlyStream;
    }

    /// <summary>
    /// Gets the <see cref="AudioOnlyStreamInfo"/> of the first matching <see cref="Container"/> at the best quality.
    /// </summary>
    /// <param name="container">The prefered container.</param>
    /// <returns>The <see cref="AudioOnlyStreamInfo"/> matching the <see cref="Container"/> or with the best bitrate.</returns>
    public AudioOnlyStreamInfo GetAudioStreamByPreferenceOrHighestBitrate(Container container)
    {
        var audioOnlyStreams = _streamManifest.GetAudioOnlyStreams();

        AudioOnlyStreamInfo? stream = (AudioOnlyStreamInfo?) audioOnlyStreams
            .Where(stream => stream.Container == container)
            .TryGetWithHighestBitrate();

        stream ??= (AudioOnlyStreamInfo)audioOnlyStreams.GetWithHighestBitrate();

        return stream;
    }
}
