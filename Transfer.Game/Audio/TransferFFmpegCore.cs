using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Exceptions;
using JetBrains.Annotations;
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
    /// <param name="fileParams">Substitute arguments.</param>
    /// <param name="customArguments">Custom arguments for editing with FFmpeg</param>
    /// <returns>Path to audio</returns>
    protected internal async Task<string> Conversion(string video, TransferConfigManager transferConfig, [NotNull] FileParams fileParams, params string[] customArguments)
    {
        string outputPath = fileParams?.OutputPath ?? Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(video));
        outputPath = Path.Combine(outputPath, fileParams.AudioFileName);
        CONVERSION_STATUS.Value = "Checking output path";
        CONVERSION_STATUS.Value = $"Start of conversion. Input: {video}, Output: {outputPath}, Arguments: [{fileParams?.ToString() ?? " "}]";

        string arguments = customArguments?.Length > 0 ? string.Join(" ", customArguments) : "";

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
                                                              .WithAudioBitrate(fileParams.Bitrate)
                                                              .WithAudioCodec(FFMpeg.GetCodec(fileParams?.AudioCodec.ToString()))
                                                              .WithVideoCodec(FFMpeg.GetCodec(transferConfig.Get<VideoCodecs>(TransferOptions.VideoCodec).ToString()))
                                                              .WithCustomArgument(
                                                                  $"-threads {transferConfig.Get<int>(TransferOptions.Threads)} " +
                                                                  "-max_muxing_queue_size 4196 " +
                                                                  $"{FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoSpeedUp, fileParams?.Rate)}")
                                                              .WithCustomArgument(arguments)
                                                              .WithFastStart())
                  .ProcessAsynchronously();
            DateTime endTime = DateTime.Now;
            CONVERSION_STATUS.Value = "Conversion completed";
            var fileSize = new FileInfo(outputPath).Length;
            Logger.Log($"Conversion completed successfully. Output: {outputPath}, Time for creating: {endTime - startTime}; File size: {fileSize}");

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
