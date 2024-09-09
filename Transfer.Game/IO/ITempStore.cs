using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Platform;
using Transfer.Game.Audio.Extensions;

namespace Transfer.Game.IO
{
    public interface ITempStore<T>  where T : class
    {
        /// <summary>
        /// Getting a track from video
        /// </summary>
        /// <param name="pathToVideo">Path to video</param>
        /// <param name="storage">Storage</param>
        /// <param name="audioExtension">Extension for audio</param>
        /// <returns>T</returns>
        public Task<T> GetTrackAsync(string pathToVideo, Storage storage, AudioExtension audioExtension = AudioExtension.mp3);
    }
}
