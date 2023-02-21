using YoutubeDownloader;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello to User!");
        Console.WriteLine("Please enter folder path where to save the Video/Audio/Playlist");
        string path = Console.ReadLine()!;

        Console.WriteLine("Download options are: Playlist / Video / Audio / Video + Audio");
        Console.WriteLine("Please enter type");

        string type = Console.ReadLine()!.ToLower();

        Console.WriteLine("Put the youtube link:");
        string youtubeLink = Console.ReadLine()!;

        var ytDownloader = new Downloader(path, type, youtubeLink);

        Console.WriteLine("Downloading... ");
        await ytDownloader.Download();
    }

}