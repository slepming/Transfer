using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Audio.Track;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Arguments;
using FFMpegCore.Pipes;
using osu.Framework.Platform;
using System.Threading;
using FFMpegCore.Exceptions;
using osu.Framework.Logging;
using Transfer.Game.IO;
using Transfer.Game.Audio.Extensions;
using Transfer.Game.Graphics;


namespace Transfer.Game.Audio
{
    public class AudioExtractorCore
    {
        /// <summary>
        /// Extract audio from video
        /// </summary>
        /// <param name="video">Path to video</param>
        /// <param name="storage">Storage</param>
        /// <param name="audioExtension">Extension for video or audio, use with <see cref="AudioExtensionHelper"></see></param>
        /// <returns>Path to audio</returns>
        public async Task<string> Extract(string video, Storage storage, AudioExtension audioExtension = AudioExtension.mp3)
        {
            string outputPath =  Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(video)}.{AudioExtensionHelper.GetExtensionString(audioExtension)}");
            try
            {
                await FFMpegArguments
                .FromFileInput(video)
                .OutputToFile(outputPath, false, options => options.WithAudioCodec(AudioCodec.LibMp3Lame).WithAudioBitrate(128)).ProcessAsynchronously();


                return outputPath;
            } catch(FFMpegException ffmpegEx)
            {
                if(ffmpegEx.Message == "Output file already exists and overwrite is disabled")
                {
                    Logger.Log("File exists, return");
                    return outputPath;
                }
                throw;
            } catch(Exception ex)
            {
                Logger.Error(ex, "FFMpeg Exception");
                throw;
            }
        }


    }
}
