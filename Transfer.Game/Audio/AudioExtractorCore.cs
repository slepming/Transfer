using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Exceptions;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using Transfer.Game.Audio.Extensions;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;


namespace Transfer.Game.Audio
{
    public class AudioExtractorCore
    {


        /// <summary>
        /// Extract audio from video
        /// </summary>
        /// <param name="video">Path to video</param>
        /// <param name="transferConfig">Config</param>
        /// <returns>Path to audio</returns>
        public static async Task<string> Extract(string video, TransferConfigManager transferConfig)
        {
            var outputPath = Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(video)).ToLower()}.mp3");
            try
            {
                Logger.Log(transferConfig.Get<Codecs>(TransferOptions.AudioCodec).ToString());
                await FFMpegArguments
                .FromFileInput(video)
                .OutputToFile(outputPath, false, options => options
                    .WithAudioBitrate(transferConfig.Get<int>(TransferOptions.AudioBitrate))
                    .WithAudioCodec(FFMpeg.GetCodec(transferConfig.Get<Codecs>(TransferOptions.AudioCodec).ToString()))
                    .WithFastStart())
                .ProcessAsynchronously();


                return outputPath;
            }
            catch (FFMpegException ffmpegEx)
            {
                if (ffmpegEx.Message == "Output file already exists and overwrite is disabled")
                {
                    Logger.Error(ffmpegEx, "File exists, return");
                    return outputPath;
                }
                Logger.Error(ffmpegEx, $"Audio equal null:");
                return outputPath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "FFMpeg Exception");
                throw;
            }
        }


    }
}
