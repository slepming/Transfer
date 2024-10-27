using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FFmpeg.NET.Exceptions;
using FFMpegCore.Exceptions;
using HidSharp.Reports.Units;
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
        private AudioManager audioManager;

        public TempStore(AudioManager audioManager) => this.audioManager = audioManager;

        
        public virtual async Task<T> CreateaAndGetTrackAsync(string path, Storage storage, AudioExtension audioExtension = AudioExtension.mp3)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (audioManager == null) throw new ArgumentNullException(nameof(audioManager));
            var audioName = $"{Path.GetFileNameWithoutExtension(path)}.{AudioExtensionHelper.GetExtensionString(audioExtension)}";
            Logger.Log($"Create file with name {audioName}");
            if(storage.GetFiles(storage.GetFullPath(@"")).Contains(audioName))
            {
                IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
                return audioManager.GetTrackStore(resourceStore).Get(audioName) as  T;
            }

            try
            {
                string pathToFile = await audioExtractorCore.Extract(path);
                if (!File.Exists(path))
                {
                    Logger.Error(new Exception(), $"{path} does not exists - {audioName} canceled");
                    throw new Exception($"{path} does not exists - {audioName} canceled");
                }
                using (Stream file = new FileStream(pathToFile,FileMode.Open))
                {
                    using (var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(audioFile);
                    }
                    using (var videoFile = storage.GetStream(Path.GetFileName(path), FileAccess.Write, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(videoFile);
                    }
                }
                IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
                File.Delete(pathToFile);
                return audioManager.GetTrackStore(resourceStore).Get(audioName) as T;
            }
            catch(FFMpegException)
            {
                throw;
            }
            catch(Exception ex)
            {
                Logger.Error(ex, $"{GetType().Name} error");
                throw;
            }

        }

        /// <summary>
        /// Extract audio and puts it is in the temp directory
        /// </summary>
        /// <param name="path">path to video file</param>
        /// <param name="storage"></param>
        /// <param name="audioExtension">Type audio extensions(using with <see cref="AudioExtensionHelper"></see>)</param>
        /// <returns></returns>
        public virtual async Task CreateTrackInStorageAsync(string path, Storage storage, AudioExtension audioExtension = AudioExtension.mp3)
        {
            if (path == null) Logger.Error(new ArgumentNullException(nameof(path)), "Path to video can not be null");
            var audioName = $"{Path.GetFileNameWithoutExtension(path)}.{AudioExtensionHelper.GetExtensionString(audioExtension)}";
            if (storage.GetFiles(storage.GetFullPath(@"")).Contains(audioName))
            {
                return;
            }
            try
            {
                string pathToAudio = await audioExtractorCore.Extract(path);
                if (!File.Exists(path))
                {
                    Logger.Error(new Exception(), $"{path} does not exists - {audioName} canceled");
                    throw new Exception($"{path} does not exists - {audioName} canceled");
                }
                using(Stream file = new FileStream(path, FileMode.Open))
                {
                    using (var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(audioFile);
                    }
                    using (var videoFile = storage.GetStream(Path.GetFileName(path), FileAccess.Write, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(videoFile);
                    }


                }
                return;
            }
            catch (FFmpegException f)
            {
                Logger.Error(f, "Video extraction error due to FFmpeg");
                throw;
            }
            catch
            {
                throw;
            }
        }


    }




}
