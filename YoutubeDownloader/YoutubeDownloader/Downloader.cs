using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;
using Container = YoutubeExplode.Videos.Streams.Container;

namespace YoutubeDownloader
{
    public class Downloader
    {
        public string Path { get; set; }
        public string Type { get; set; }
        public string YoutubeLink { get; set; }
        private YoutubeClient YtClient { get; set; }

        public Downloader(string path, string type, string youtubeLink)
        {
            Path = path;
            Type = type;
            YoutubeLink = youtubeLink;
            YtClient = new YoutubeClient();
    }

        public async Task Download()
        {
            if (this.Type.ToLower() == "audio")
            {
                await DownloadAudioOnly(this.Path!, this.YtClient, this.YoutubeLink!);

            }
            else if (this.Type.ToLower() == "video")
            {
                await DownloadVideoOnly(this.Path!, this.YtClient, this.YoutubeLink!);

            }
            else if (this.Type.ToLower() == "video + audio")
            {
                await DownloadVideoWithAudio(this.Path!, this.YtClient, this.YoutubeLink!);

            }
            else if (this.Type.ToLower() == "playlist")
            {
                await DownloadPlaylist(this.Path!, this.YtClient, this.YoutubeLink!);
            }
        }

        private static async Task DownloadVideoWithAudio(string path, YoutubeClient youtube, string link)
        {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"{link}");

            var video = await youtube.Videos.GetAsync($"{link}");

            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

            await youtube.Videos.Streams.DownloadAsync(streamInfo, @$"{path}\{video.Title}.{streamInfo.Container}");

            Console.WriteLine($"{video.Title} is downloaded!");
        }

        private static async Task DownloadVideoOnly(string path, YoutubeClient youtube, string link)
        {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"{link}");

            var video = await youtube.Videos.GetAsync($"{link}");

            var streamInfo = streamManifest
            .GetVideoOnlyStreams()
            .Where(s => s.Container == Container.Mp4)
            .GetWithHighestVideoQuality();

            await youtube.Videos.Streams.DownloadAsync(streamInfo, @$"{path}\{video.Title}.{streamInfo.Container}");

            Console.WriteLine($"{video.Title} is downloaded!");

        }

        private static async Task DownloadAudioOnly(string path, YoutubeClient youtube, string link)
        {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"{link}");

            var video = await youtube.Videos.GetAsync($"{link}");

            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            await youtube.Videos.Streams.DownloadAsync(streamInfo, @$"{path}\{video.Title}.{streamInfo.Container}");

            Console.WriteLine($"{video.Title} is downloaded!");

        }

        private static async Task DownloadPlaylist(string path, YoutubeClient youtube, string link)
        {
            var videosSubset = await youtube.Playlists
            .GetVideosAsync($"{link}");

            foreach (var video in videosSubset)
            {
                var url = video.Url;
                await DownloadVideoWithAudio(path, youtube, url);
                Console.WriteLine($"{video.Title} is downloaded!");
            }
        }
    }
}
