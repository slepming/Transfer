using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Exceptions;
using osu.Framework.Bindables;
using osu.Framework.Logging;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio;

public abstract class TransferFFmpegCore
{
    public static readonly Bindable<string> CONVERSION_STATUS = new();

    /// <summary>
    /// Extract audio from video
    /// </summary>
    /// <param name="video">Path to video</param>
    /// <param name="transferConfig">Config</param>
    /// <param name="customArguments">Here you can enter your arguments to modify in FFmpeg</param>
    /// <param name="outputExtension">Output file extension(Start with '.'). If output path is not null, then output extension it's no use</param>
    /// <returns>Path to audio</returns>
    protected internal async Task<string> Conversion(string video, TransferConfigManager transferConfig, string customArguments = null, string outputExtension = null)
    {
        string outputPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(video));
        CONVERSION_STATUS.Value = "Checking output path";
        CONVERSION_STATUS.Value = $"Start of conversion. Input: {video}, Output: {outputPath}, Arguments: {customArguments ?? "null"}";

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
            Logger.Log($"Conversion started at {outputPath}");
            DateTime startTime = DateTime.Now;
            await FFMpegArguments
                  .FromFileInput(video)
                  .OutputToFile(outputPath, false, options => options
                                                              .WithAudioBitrate(transferConfig.Get<int>(TransferOptions.AudioBitrate))
                                                              .WithAudioCodec(FFMpeg.GetCodec(transferConfig.Get<AudioCodecs>(TransferOptions.AudioCodec).ToString()))
                                                              .WithVideoCodec(FFMpeg.GetCodec(transferConfig.Get<VideoCodecs>(TransferOptions.VideoCodec).ToString()))
                                                              .ForceFormat(outputExtension)
                                                              .WithCustomArgument(
                                                                  $"-threads {transferConfig.Get<int>(TransferOptions.Threads)} " +
                                                                  "-max_muxing_queue_size 2048 ")
                                                              .WithCustomArgument(customArguments ?? "")
                                                              .ForceFormat("mp3")
                                                              .WithFastStart())
                  .ProcessAsynchronously();
            DateTime endTime = DateTime.Now;
            CONVERSION_STATUS.Value = "Conversion completed";
            var fileSize = new FileInfo(outputPath).Length;
            Logger.Log($"Conversion completed successfully. Output: {outputPath}, Time: {endTime - startTime}; File size: {fileSize}");

            if (!File.Exists(outputPath) || fileSize == 0)
            {
                throw new Exception("FFmpeg conversion resulted in empty file");
            }

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

            CONVERSION_STATUS.Value = "FFmpeg Error";
            Logger.Error(ffmpegEx, "FFmpeg error");
            return outputPath;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Exception during conversion");
            CONVERSION_STATUS.Value = "Unhandled exception";
            throw;
        }
    }
}
