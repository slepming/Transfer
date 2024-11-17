using System;
using System.Collections.Generic;
using System.IO;
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
            if (!File.Exists(pathToFile)) return null;
#if DEBUG
            Logger.Log("Current directory: " + Environment.CurrentDirectory, LoggingTarget.Information, LogLevel.Verbose);
            Logger.Log($"Running in {(Environment.Is64BitProcess ? "64" : "32")}-bit mode.", LoggingTarget.Information, LogLevel.Verbose);
#endif
            Logger.Log($"LIBAVFORMAT Version: {ffmpeg.LIBAVFORMAT_VERSION_MAJOR}.{ffmpeg.LIBAVFORMAT_VERSION_MINOR}", LoggingTarget.Information,LogLevel.Verbose);
            return showVideoMetaData(pathToFile);
        }

        private static Dictionary<string,string> showVideoMetaData(string pathToFile)
        {
            AVFormatContext* fmt_ctx = null;
            try
            {
                do
                {
                    Logger.Log("FFmpeg avformat open input check!", LoggingTarget.Information, LogLevel.Verbose);
                    int ret = ffmpeg.avformat_open_input(&fmt_ctx,pathToFile, null,null);

                    if(ret != 0) break;
                    Logger.Log("FFmpeg avformat find stream info check!", LoggingTarget.Information, LogLevel.Verbose);
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
                    Logger.Log("Data collection is complete", LoggingTarget.Information, LogLevel.Verbose);
                    return pairs;
                } while(false);
                if(fmt_ctx != null)
                {
                    ffmpeg.avformat_close_input(&fmt_ctx);
                }
                Logger.Log("Dictionary is null", LoggingTarget.Information, LogLevel.Verbose);
                return new Dictionary<string,string>{ {"null", "null"} };
            } catch(Exception ex)
            {
                Logger.Error(ex, "FFmpeg Metadata error");
                throw;
            }
        }


    }
}
