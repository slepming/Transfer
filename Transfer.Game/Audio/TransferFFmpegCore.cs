using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Exceptions;
using osu.Framework.Bindables;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;


namespace Transfer.Game.Audio
{
    public class TransferFFmpegCore
    {
        public static readonly Bindable<string> CONVERSION_STATUS = new Bindable<string>();

        /// <summary>
        /// Extract audio from video
        /// </summary>
        /// <param name="video">Path to video</param>
        /// <param name="transferConfig">Config</param>
        /// <param name="outputPath">The path to which the file will be written</param>
        /// <param name="customArguments">Here you can enter your arguments to modify in FFmpeg</param>
        /// <param name="outputExtension">Output file extension(Start with '.'). If output path is not null, then output extension it's no use</param>
        /// <returns>Path to audio</returns>
        public static async Task<string> Conversion(string video, TransferConfigManager transferConfig, string outputPath = null, string customArguments = null, string outputExtension = ".mp3")
        {
            CONVERSION_STATUS.Value = "Checking output path";
            outputPath ??= Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(video)).ToLower()}{outputExtension}");
            CONVERSION_STATUS.Value = "Argument validation";
            try
            {
                Logger.Log($"Start of conversion. Input: {video}, Output: {outputPath}, Arguments: {customArguments}");
                CONVERSION_STATUS.Value = "Start conversion";
                await FFMpegArguments
                    .FromFileInput(video)
                    .OutputToFile(outputPath, false, options => options
                        .WithAudioBitrate(transferConfig.Get<int>(TransferOptions.AudioBitrate))
                        .WithAudioCodec(FFMpeg.GetCodec(transferConfig.Get<AudioCodecs>(TransferOptions.AudioCodec).ToString()))
                        .WithVideoCodec(FFMpeg.GetCodec(transferConfig.Get<VideoCodecs>(TransferOptions.VideoCodec).ToString()))
                        .WithCustomArgument(customArguments ?? "")
                        .WithFastStart())
                    .ProcessAsynchronously();
                CONVERSION_STATUS.Value = "Conversion completed";
                Logger.Log($"Conversion completed successfully. Output: {outputPath}");
                return outputPath;
            }
            catch (FFMpegException ffmpegEx)
            {
                Logger.Log("FFMpegException during conversion");
                if (ffmpegEx.Message == "Output file already exists and overwrite is disabled")
                {
                    Logger.Log("File exists, return");
                    CONVERSION_STATUS.Value = "File exists, return";
                    return outputPath;
                }
                CONVERSION_STATUS.Value = "FFmpeg Error(can be false)";
                Logger.Error(ffmpegEx, "FFmpeg error(can be false)");
                return outputPath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception during conversion");
                throw;
            }
        }



    }
}
