using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFmpeg.NET.Exceptions;
using FFMpegCore.Exceptions;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Audio;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;

namespace Transfer.Game.IO
{
    public class AudioExtract<T>(TransferConfigManager transferConfigManager) : IAudioExtract<T> where T : Track
    {


        public virtual async Task<T> CreateaAndGetTrackAsync(string path, Storage storage, AudioManager audioManager)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (audioManager == null) throw new ArgumentNullException(nameof(audioManager));
            var audioName = $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(path)).ToLower()}.mp3";

            IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(storage);
            if (storage.Exists(audioName))
            {
                return audioManager.GetTrackStore(resourceStore).Get(audioName) as T;
            }

            try
            {
                string pathToFile = await AudioExtractorCore.Extract(path, transferConfigManager);
                if (!File.Exists(path))
                {
                    Logger.Error(new Exception(), $"{path} does not exists - {audioName} canceled");
                    throw new Exception($"{path} does not exists - {audioName} canceled");
                }
                using (Stream file = new FileStream(pathToFile, FileMode.Open))
                {
                    using (var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.OpenOrCreate))
                    {
                        Logger.Log($"Files puts in {storage.GetFullPath(audioName)}");
                        await file.CopyToAsync(audioFile);
                    }
                    using (var videoFile = storage.GetStream(Path.GetFileName(path), FileAccess.Write, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(videoFile);
                    }
                }
                File.Delete(pathToFile);
                Logger.Log($"Create file with name {audioName}");
                if (!storage.Exists(audioName)) throw new FileNotFoundException("Audio not found");

                return audioManager.GetTrackStore(resourceStore).Get(audioName) as T;
            }
            catch (FFMpegException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException) return null;
                Logger.Error(ex, $"{GetType().Name} error");
                throw;
            }

        }

        public virtual async Task CreateTrackInStorageAsync(string path, Storage storage)
        {
            if (path == null) Logger.Error(new ArgumentNullException(nameof(path)), "Path to video can not be null");
            var audioName = $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(path)).ToLower()}.mp3";
            var videoName = $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(path)).ToLower()}";
            if (storage.GetFiles(storage.GetFullPath(@"")).Contains(audioName))
            {
                Logger.Log("File exist in local storage");
                return;
            }
            try
            {
                string pathToAudio = await AudioExtractorCore.Extract(path, transferConfigManager);
                if (!File.Exists(path))
                {
                    Logger.Error(new Exception(), $"{path} does not exists - {audioName} canceled");
                    throw new Exception($"{path} does not exists - {audioName} canceled");
                }
                using (Stream file = new FileStream(path, FileMode.Open))
                {
                    using (var audioFile = storage.GetStream(audioName, FileAccess.Write, FileMode.OpenOrCreate))
                        await file.CopyToAsync(audioFile);

                    using (var videoFile = storage.GetStream(videoName, FileAccess.Write, FileMode.OpenOrCreate))
                        await file.CopyToAsync(videoFile);
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
