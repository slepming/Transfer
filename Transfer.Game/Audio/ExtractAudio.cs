using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using osu.Framework;
using osu.Framework.Logging;
using FFmpeg.NET;
using System.Runtime.Versioning;
using System.Threading;
using FFmpeg.NET.Events;



namespace Transfer.Game.Audio
{
    [Obsolete("Is obsolete, please use AuduoExtractorCore")]
    public class ExtractAudio : FFmpegLogger
    {
        /// <summary>
        /// Extract audio(in mp3) from video
        /// </summary>
        /// <param name="videoPath">Path to video file</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Path to audio file</returns>
        public async Task<string> ExtractAudioFromVideoAsync(string videoPath, CancellationToken cancellationToken)
        {

            var ffmpegPath = getFFmpegPath();
            var ffmpeg = new Engine(ffmpegPath);
            var inputFile = new InputFile(videoPath);
            // Get temp path and temp file name
            var outputTempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.mp3");
            var outputFile = new OutputFile(outputTempPath);
            var conversionOptions = new ConversionOptions
            {
                AudioBitRate = 128
            };

            try
            {
                await ffmpeg.ConvertAsync(inputFile, outputFile, conversionOptions,cancellationToken);

                if(File.Exists(outputTempPath))
                {
                    Logger.Log("Extract success " + outputTempPath);
                    return outputTempPath;
                }
                else{
                    Logger.Log("Extract lose");
                    return null;
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Extract lose");
                Logger.Error(ex,"Extract fail");
                return null;
            }


        }

        /// <summary>
        /// Extract audio(in mp3) from video
        /// </summary>
        /// <param name="videoPath">Path to video file</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Stream">Stream</see></returns>
        public async Task<Stream> ExtractAudioFromVideoAsyncToStream(string videoPath, CancellationToken cancellationToken)
        {
            var ffmpegPath = getFFmpegPath();
            var ffmpeg = new Engine(ffmpegPath);
            var inputFile = new InputFile(videoPath);
            var conversionOptions = new ConversionOptions
            {
                AudioBitRate = 128
            };
            ffmpeg.Progress += OnProgress;
            ffmpeg.Data += OnData;
            ffmpeg.Error += OnError;
            ffmpeg.Complete += OnComplete;

            try
            {
                Stream stream;
                cancellationToken.ThrowIfCancellationRequested();
                stream = await ffmpeg.ConvertAsync(inputFile, conversionOptions, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Convert audio exception");
                return null;
            }
            finally
            {
                ffmpeg.Progress -= OnProgress;
                ffmpeg.Data -= OnData;
                ffmpeg.Error -= OnError;
                ffmpeg.Complete -= OnComplete;
            }

        }
        private string getFFmpegPath()
        {
            switch(RuntimeInfo.OS)
            {
                case RuntimeInfo.Platform.Windows: return Path.Combine(Environment.CurrentDirectory, "ffmpeg", "bin", "ffmpeg.exe");
                case var n when n == RuntimeInfo.Platform.Linux || n == RuntimeInfo.Platform.macOS: return "ffmpeg";
                default: Logger.Error(new PlatformNotSupportedException(), "Your OS unsupported"); throw new PlatformNotSupportedException("Your OS unsupported");
            }
        }

    }
}
