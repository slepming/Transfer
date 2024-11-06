using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Enums;
using osu.Framework.Platform;
using FFMpegCore.Exceptions;
using osu.Framework.Logging;
using Transfer.Game.Audio.Extensions;
using System.Security.Cryptography;
using System.Text;
using Transfer.Game.Extensions;


namespace Transfer.Game.Audio
{
    public class AudioExtractorCore
    {
        /// <summary>
        /// Extract audio from video
        /// </summary>
        /// <param name="video">Path to video</param>
        /// <param name="audioExtension">Extension for video or audio, use with <see cref="AudioExtensionHelper"></see></param>
        /// <returns>Path to audio</returns>
        public async Task<string> Extract(string video, AudioExtension audioExtension = AudioExtension.mp3)
        {
            string outputPath =  Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(video))}.{AudioExtensionHelper.GetExtensionString(audioExtension)}");
            try
            {
                await FFMpegArguments
                .FromFileInput(video)
                .OutputToFile(outputPath, false, options => options
                    .WithAudioBitrate(128)
                    .WithAudioCodec(AudioCodec.LibMp3Lame)
                    .WithFastStart())
                .ProcessAsynchronously();


                return outputPath;
            } catch(FFMpegException ffmpegEx)
            {
                if(ffmpegEx.Message == "Output file already exists and overwrite is disabled")
                {
                    Logger.Error(ffmpegEx,"File exists, return");
                    return outputPath;
                }
                Logger.Log($"Unhandled message: {ffmpegEx.StackTrace} \n");
                return outputPath;
            } catch(Exception ex)
            {
                Logger.Error(ex, "FFMpeg Exception");
                throw;
            }
        }


    }
}
