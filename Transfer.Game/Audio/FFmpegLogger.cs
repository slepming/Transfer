using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.NET.Events;
using osu.Framework.Logging;

namespace Transfer.Game.Audio
{
    public abstract class FFmpegLogger
    {
        protected virtual void OnProgress(object sender, ConversionProgressEventArgs e)
        {
            Logger.Log($"[{e.Input.Name} => {e.Output.Name}]");
            Logger.Log($"Bitrate: {e.Bitrate}");
            Logger.Log($"Fps: {e.Fps}");
            Logger.Log($"Frame: {e.Frame}");
            Logger.Log($"ProcessedDuration: {e.ProcessedDuration}");
            Logger.Log($"Size: {e.SizeKb} kb");
            Logger.Log($"TotalDuration: {e.TotalDuration}\n");
        }

        protected virtual void OnData(object sender, ConversionDataEventArgs e)
        {
            Logger.Log($"[{e.Input.Name} => {e.Output.Name}]: {e.Data}");
        }

        protected virtual void OnComplete(object sender, ConversionCompleteEventArgs e)
        {
            Logger.Log($"Completed conversion from {e.Input.Name} to {e.Output.Name}");
        }

        protected virtual void OnError(object sender, ConversionErrorEventArgs e)
        {
            Logger.Log($"[{e.Input.Name} => {e.Output.Name}]: Error: {e.Exception.ExitCode}\n{e.Exception.InnerException}");
        }
    }
}
