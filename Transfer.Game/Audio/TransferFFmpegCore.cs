using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Exceptions;
using osu.Framework.Bindables;
using osu.Framework.Logging;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio
{
    public abstract class TransferFFmpegCore
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
        protected internal async Task<string> Conversion(string video, TransferConfigManager transferConfig, string outputPath, string customArguments = null, string outputExtension = null)
        {
            CONVERSION_STATUS.Value = $"Start of conversion. Input: {video}, Output: {outputPath}, Arguments: {customArguments ?? "null"}";
            CONVERSION_STATUS.Value = "Checking output path";

            if (string.IsNullOrEmpty(outputPath))
            {
                throw new ArgumentNullException(nameof(outputPath));
            }

            CONVERSION_STATUS.Value = "Argument validation";

            if (!File.Exists(video))
            {
                throw new FileNotFoundException("Input video file not found.", video);
            }

            try
            {
                CONVERSION_STATUS.Value = "Start conversion";
                DateTime startTime = DateTime.Now;
                await FFMpegArguments
                      .FromFileInput(video)
                      .OutputToFile(outputPath, false, options => options
                                                                  .WithAudioBitrate(transferConfig.Get<int>(TransferOptions.AudioBitrate))
                                                                  .WithAudioCodec(FFMpeg.GetCodec(transferConfig.Get<AudioCodecs>(TransferOptions.AudioCodec).ToString()))
                                                                  .WithVideoCodec(FFMpeg.GetCodec(transferConfig.Get<VideoCodecs>(TransferOptions.VideoCodec).ToString()))
                                                                  .ForceFormat(outputExtension)
                                                                  .WithCustomArgument($"-preset {transferConfig.Get<Presets>(TransferOptions.Preset)} " +
                                                                                      $"-threads {transferConfig.Get<int>(TransferOptions.Threads)} " +
                                                                                      $"-max_muxing_queue_size 2048" +
                                                                                      $"-crf {transferConfig.Get<int>(TransferOptions.ConstantRateFactor)} " +
                                                                                      $"-profile:v {transferConfig.Get<Profiles>(TransferOptions.Profile)}")
                                                                  .WithCustomArgument(customArguments ?? "")
                                                                  .WithFastStart())
                      .ProcessAsynchronously();
                DateTime endTime = DateTime.Now;
                CONVERSION_STATUS.Value = "Conversion completed";
                Logger.Log($"Conversion completed successfully. Output: {outputPath}, Time: {endTime - startTime}");
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
