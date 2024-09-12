using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using osu.Framework.Logging;

namespace Transfer.Game.Extensions
{

    /// <summary>
    /// Get metadata from path
    /// </summary>
    public unsafe class FFmpegMetadata
    {
        /// <summary>
        /// Get data from Metadata
        /// </summary>
        /// <param name="pathToFile">Path to file</param>
        /// <returns>Dictionary with Type:Key, Value:Value</returns>
        public static Dictionary<string,string> GetData(string pathToFile)
        {
#if DEBUG
            Logger.Log("Current directory: " + Environment.CurrentDirectory, LoggingTarget.Runtime, LogLevel.Important);
            Logger.Log($"Running in {(Environment.Is64BitProcess ? "64" : "32")}-bit mode.", LoggingTarget.Runtime, LogLevel.Important);
            Logger.Log($"FFmpeg version info: {ffmpeg.av_version_info()}", LoggingTarget.Runtime, LogLevel.Important);
#endif
            Logger.Log($"LIBAVFORMAT Version: {ffmpeg.LIBAVFORMAT_VERSION_MAJOR}.{ffmpeg.LIBAVFORMAT_VERSION_MINOR}");
            return metaData(pathToFile);
        }

        private static Dictionary<string,string> metaData(string pathToFile)
        {
            AVFormatContext* fmt_ctx = null;
            try
            {
                do
                {
                    Logger.Log("FFmpeg avformat open input check!", LoggingTarget.Runtime, LogLevel.Important);
                    int ret = ffmpeg.avformat_open_input(&fmt_ctx,pathToFile, null,null);

                    if(ret != 0) break;
                    Logger.Log("FFmpeg avformat find stream info check!", LoggingTarget.Runtime, LogLevel.Important);
                    ret = ffmpeg.avformat_find_stream_info(fmt_ctx,null);
                    if(ret < 0)
                    {
                        Logger.Error(new Exception(), "Cannot find stream information");
                        return new Dictionary<string,string>{ {"null", "null"} };
                    }
                    AVDictionaryEntry* tag = null;

                    Dictionary<string,string> pairs = new Dictionary<string, string>();
                    while((tag = ffmpeg.av_dict_get(fmt_ctx -> metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
                    {
                        string key = Marshal.PtrToStringAnsi(new IntPtr(tag->key));
                        string value = Marshal.PtrToStringAnsi(new IntPtr(tag->value));


                        pairs.Add(key,value);
                    }
                    Logger.Log("Data collection is complete", LoggingTarget.Runtime, LogLevel.Important);
                    return pairs;
                } while(false);
                if(fmt_ctx != null)
                {
                    ffmpeg.avformat_close_input(&fmt_ctx);
                }
                Logger.Log("Dictionary is null", LoggingTarget.Runtime, LogLevel.Important);
                return new Dictionary<string,string>{ {"null", "null"} };
            } catch(Exception ex)
            {
                Logger.Error(ex, "FFmpeg Metadata error");
                throw;
            }
        }

    }
}
