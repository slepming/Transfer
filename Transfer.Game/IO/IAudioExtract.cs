using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Platform;
using Transfer.Game.Audio.Extensions;

namespace Transfer.Game.IO
{
    public interface IAudioExtract<T>  where T : class
    {
        /// <summary>
        /// Extract audio from video create audio in storage,and convertion in <see cref="Track"></see>
        /// </summary>
        /// <param name="path">Path to video</param>
        /// <param name="storage">Storage</param>
        /// <param name="audioManager"></param>
        /// <returns>Track</returns>
        public Task<T> CreateaAndGetTrackAsync(string path, Storage storage, AudioManager audioManager);

        /// <summary>
        /// Extract audio and puts it is in the temp directory
        /// </summary>
        /// <param name="path">path to video file</param>
        /// <param name="storage">Storage</param>
        /// <returns></returns>
        public Task CreateTrackInStorageAsync(string path, Storage storage);
    }
}
