using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        [Obsolete]
        private ExtractAudio extractAudio = new ExtractAudio();
        private AudioExtractorCore audioExtractorCore = new();
        public AudioManager AudioManager { get; set; }
        public List<string> Extensions = new List<string>
        {
            ".mp4"
        };

        /// <summary>
        /// Get Audio from Storage
        /// </summary>
        /// <param name="pathToVideo">Path to Video</param>
        /// <param name="storage"></param>
        /// <param name="audioExtension"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        [Obsolete("The development of this method is abandoned, use GetTrackAsync() | To be deleted")]
        public virtual async Task<T> GetAsync(string pathToVideo, Storage storage, AudioExtension audioExtension = AudioExtension.mp3,CancellationToken cancellationToken = default)
        {
            if(pathToVideo == null) Logger.Error(new ArgumentNullException(nameof(pathToVideo)), "PathToVideo cannot be null");
            CancellationTokenSource cts = new CancellationTokenSource();

            if(Extensions.Contains(Path.GetExtension(pathToVideo)))
            {
                try
                {
                    var audioName = $"{Guid.NewGuid()}.{audioExtension}";
                    Logger.Log("New audio filename in Temp: " + audioName);
                    using(var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.Create))
                    using(Stream stream = await extractAudio.ExtractAudioFromVideoAsyncToStream(pathToVideo, cts.Token))
                        await stream.CopyToAsync(audioFile);
                    IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
                    return AudioManager.GetTrackStore(resourceStore).Get(audioName) as T;
                } catch(Exception ex)
                {
                    Logger.Error(ex, "TempStore Exception");
                }
            }
            else
            {
                Logger.Log("Have not extension mp4");
                return null;
            }
            return null;

        }

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

            string pathToFile = await audioExtractorCore.Extract(pathToVideo, storage);
            if(!File.Exists(pathToFile)) Logger.Error(new Exception(), $"{pathToFile} does not exists - {audioName} canceled");
            try
            {
                using(FileStream file = new FileStream(pathToFile,FileMode.Open))
                {
                    using(var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.OpenOrCreate))
                    await file.CopyToAsync(audioFile);
                }
                IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
                File.Delete(pathToFile);
                return AudioManager.GetTrackStore(resourceStore).Get(audioName) as T;
            }
            catch(Exception ex)
            {
                Logger.Error(ex, $"{GetType().Name} error");
                return null!;
            }

        }


    }




}
