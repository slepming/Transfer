using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using osu.Framework.Logging;

namespace Transfer.Game.Extensions
{
    public unsafe class FFmpegMetadata
    {
        public static Dictionary<string,string> GetData(string pathToFile)
        {
#if DEBUG
        Logger.Log("Current directory " + Environment.CurrentDirectory);
        Logger.Log($"Running in {(Environment.Is64BitProcess ? "64" : "32")}-bit mode.");
        Logger.Log($"FFmpeg version info: {ffmpeg.av_version_info()}");
#endif
        Logger.Log($"LIBAVFORMAT Version: {ffmpeg.LIBAVFORMAT_VERSION_MAJOR}.{ffmpeg.LIBAVFORMAT_VERSION_MINOR}");
        return metaData(pathToFile);
        }

        private static Dictionary<string,string> metaData(string pathToFile)
        {
            AVFormatContext* fmt_ctx = null;
            do
            {
                Logger.Log("FFmpeg avformat open input check!");
                int ret = ffmpeg.avformat_open_input(&fmt_ctx,pathToFile, null,null);

                if(ret != 0) break;
                Logger.Log("FFmpeg avformat find stream info check!");
                ret = ffmpeg.avformat_find_stream_info(fmt_ctx,null);
                if(ret < 0)
                {
                    Logger.Error(new Exception(), "Cannot find stream information");
                    return null;
                }
                AVDictionaryEntry* tag = null;

                Dictionary<string,string> pairs = new Dictionary<string, string>();
                while((tag = ffmpeg.av_dict_get(fmt_ctx -> metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
                {
                    string key = Marshal.PtrToStringAnsi(new IntPtr(tag->key));
                    string value = Marshal.PtrToStringAnsi(new IntPtr(tag->value));


                    pairs.Add(key,value);
                }
                return pairs;
            } while(false);
            if(fmt_ctx != null)
            {
                ffmpeg.avformat_close_input(&fmt_ctx);
            }
            Logger.Log("Dictionary is null");
            return null;
        }

    }
}
