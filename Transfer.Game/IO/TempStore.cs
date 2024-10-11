using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore.Exceptions;
using OpenTabletDriver.Plugin.DependencyInjection;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Audio;
using Transfer.Game.Audio.Extensions;

namespace Transfer.Game.IO
{
    public class TempStore<T> : ITempStore<T> where T : Track
    {
        private AudioExtractorCore audioExtractorCore = new();

        public AudioManager AudioManager { get; set; }

        /// <summary>
        /// Extract audio from video and convertion in <see cref="Track"></see>
        /// </summary>
        /// <param name="pathToVideo">Path to video</param>
        /// <param name="storage">Storage for temp files</param>
        /// <param name="audioExtension">Type audio extensions(using with <see cref="AudioExtensionHelper"></see>)</param>
        /// <returns>Track</returns>
        public virtual async Task<T> GetTrackAsync(string pathToVideo, Storage storage, AudioExtension audioExtension = AudioExtension.mp3)
        {
            if(pathToVideo == null) Logger.Error(new ArgumentNullException(nameof(pathToVideo)), "Path to video can not be null");
            var audioName = $"{Path.GetFileNameWithoutExtension(pathToVideo)}.{AudioExtensionHelper.GetExtensionString(audioExtension)}";
            if(storage.GetFiles(storage.GetFullPath(@"")).Contains(audioName))
            {
                IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
                return AudioManager.GetTrackStore(resourceStore).Get(audioName) as  T;
            }

            try
            {
                string pathToFile = await audioExtractorCore.Extract(pathToVideo, storage);
                if(!File.Exists(pathToFile)) Logger.Error(new Exception(), $"{pathToFile} does not exists - {audioName} canceled");
                using(Stream file = new FileStream(pathToFile,FileMode.Open))
                {
                    using(var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.OpenOrCreate))
                    await file.CopyToAsync(audioFile);
                }
                IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
                File.Delete(pathToFile);
                return AudioManager.GetTrackStore(resourceStore).Get(audioName) as T;
            }
            catch(FFMpegException ffmpegEx)
            {
                throw;
            }
            catch(Exception ex)
            {
                Logger.Error(ex, $"{GetType().Name} error");
                return null!;
            }

        }


    }




}
