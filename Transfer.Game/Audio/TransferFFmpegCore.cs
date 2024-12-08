using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
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
        /// <returns>Path to audio</returns>
        public static async Task<string> Conversion(string video, TransferConfigManager transferConfig, string outputPath = null, string customArguments = null)
        {
            CONVERSION_STATUS.Value = "Checking output path";
            outputPath ??= Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(video)).ToLower()}.mp3");
            CONVERSION_STATUS.Value = "Argument validation";
            try
            {
                Logger.Log($"Starting conversion. Input: {video}, Output: {outputPath}, Arguments: {customArguments}");
                CONVERSION_STATUS.Value = "Start conversion";
                await FFMpegArguments
                    .FromFileInput(video)
                    .OutputToFile(outputPath, false, options => options
                        .WithAudioBitrate(transferConfig.Get<int>(TransferOptions.AudioBitrate))
                        .WithAudioCodec(FFMpeg.GetCodec(transferConfig.Get<Codecs>(TransferOptions.AudioCodec).ToString()))
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
                CONVERSION_STATUS.Value = "Audio equal null";
                Logger.Log("Audio equal null");
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
