using YoutubeExplode;
using YoutubeExplode.Converter;
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
        None = 0,
        Audio = 1,
        Video = 2,
        Caption = 4
    }

    public struct DownloadOptions
    {
        public required DownloadType Type { get; init; }
        public required Container PreferedContainer { get; init; }
    }

    private readonly YoutubeClient _client;
    private readonly YoutubeStreamManager _streamManager;

    /// <summary>
    /// The folder where the data will be saved at.
    /// </summary>
    public required DirectoryInfo SaveDirectory { get; init; }
    public required DownloadOptions Options { get; init; }

    private YoutubeDownloader(YoutubeClient client, YoutubeStreamManager streamManager)
    {
        _client = client;
        _streamManager = streamManager;
    }

    public static async Task<YoutubeDownloader> Create(DownloadOptions options, VideoId id)
    {
        YoutubeClient client = new();

        YoutubeStreamManager manager = await YoutubeStreamManager.Create(client, id);

        YoutubeDownloader downloader = new(client, manager)
        {
            Options = options,
            SaveDirectory = new DirectoryInfo(Directory.GetCurrentDirectory())
        };

        return downloader;
    }

    public async ValueTask Download()
    {
        if (Options.Type.HasFlag(DownloadType.Audio))
            await DownloadAudio(Options.PreferedContainer);

        if (Options.Type.HasFlag(DownloadType.Video))
            await DownloadVideo(Options.PreferedContainer);
    }

    private async ValueTask DownloadAudio(Container container)
    {
        var videoTask = _streamManager.GetVideo();
        AudioOnlyStreamInfo streamInfo = _streamManager.GetAudioStreamByPreferenceOrHighestBitrate(container);

        Video video = await videoTask;
        string fileName = video.Title;

        string extension = streamInfo.Container.ToString();

        string filePath = Path.Combine(SaveDirectory.FullName, $"{fileName}.{extension}");

        await _client.Videos.Streams.DownloadAsync(streamInfo, filePath);
    }

    private async ValueTask DownloadVideo(Container audioContainer)
    {
        var videoTask = _streamManager.GetVideo();
        AudioOnlyStreamInfo audioStreamInfo = _streamManager.GetAudioStreamByPreferenceOrHighestBitrate(audioContainer);
        VideoOnlyStreamInfo videoStreamInfo = _streamManager.GetBestVideoStream();

        var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };

        Video video = await videoTask;
        string fileName = video.Title;

        string extension = videoStreamInfo.Container.ToString();

        string filePath = Path.Combine(SaveDirectory.FullName, $"{fileName}.{extension}");

        ConversionRequestBuilder builder = new ConversionRequestBuilder(filePath);

        await _client.Videos.DownloadAsync(streamInfos, builder.Build());
    }
}
