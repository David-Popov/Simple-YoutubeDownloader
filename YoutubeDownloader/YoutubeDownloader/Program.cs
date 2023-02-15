using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello to User!");
        Console.WriteLine("Please enter folder path where to save the Video/Audio/Playlist");
        string path = @"D:\YoutubeDownloader";

        Console.WriteLine("Download options are: Playlist / Video / Audio / Video + Audio");
        Console.WriteLine("Please enter type");

        string type = Console.ReadLine()!.ToLower();

        Console.WriteLine("Put the youtube link:");
        string youtubeLink = Console.ReadLine()!;

        var youtube = new YoutubeClient();

        Console.WriteLine("Downloading");

        if (type == "audio")
        {
            await DownloadAudioOnly(path!, youtube, youtubeLink!);

        }
        else if (type == "video")
        {
            await DownloadVideoOnly(path!, youtube, youtubeLink!);

        }
        else if (type == "video + audio")
        {
            await DownloadVideoWithAudio(path!, youtube, youtubeLink!);

        }
        else if (type == "playlist")
        {
            await DownloadPlaylist(path!, youtube, youtubeLink!);
        }
    }

    static async Task DownloadVideoWithAudio(string path, YoutubeClient youtube, string link)
    {
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"{link}");

        var video = await youtube.Videos.GetAsync($"{link}");

        var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

        await youtube.Videos.Streams.DownloadAsync(streamInfo, @$"{path}\{video.Title}.{streamInfo.Container}");
    }

    static async Task DownloadVideoOnly(string path, YoutubeClient youtube, string link)
    {
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"{link}");

        var video = await youtube.Videos.GetAsync($"{link}");

        var streamInfo = streamManifest
        .GetVideoOnlyStreams()
        .Where(s => s.Container == Container.Mp4)
        .GetWithHighestVideoQuality();

        await youtube.Videos.Streams.DownloadAsync(streamInfo, @$"{path}\{video.Title}.{streamInfo.Container}");
    }

    static async Task DownloadAudioOnly(string path, YoutubeClient youtube, string link)
    {
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"{link}");

        var video = await youtube.Videos.GetAsync("https://youtube.com/watch?v=u_yIGGhubZs");

        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        await youtube.Videos.Streams.DownloadAsync(streamInfo, @$"{path}\{video.Title}.{streamInfo.Container}");
    }

    static async Task DownloadPlaylist(string path, YoutubeClient youtube, string link)
    {
        var videosSubset = await youtube.Playlists
        .GetVideosAsync($"{link}")
        .CollectAsync(10);

        foreach (var video in videosSubset)
        {
           var url = video.Url;
           await DownloadVideoWithAudio(path,youtube,url);
        }
    }


}