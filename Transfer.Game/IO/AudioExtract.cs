using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMpegCore.Exceptions;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Audio;
using Transfer.Game.Audio.ConversionModels;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;

namespace Transfer.Game.IO
{
    public class AudioExtract<T>(TransferConfigManager transferConfigManager) : IAudioExtract<T> where T : Track
    {


        public async Task<T> CreateaAndGetTrackAsync(string path, Storage storage, AudioManager audioManager, string outputPath = null,string arguments = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (audioManager == null) throw new ArgumentNullException(nameof(audioManager));

            outputPath ??= getHashName(path, "mp3");
            var resourceStore = new StorageBackedResourceStore(storage);

            if (storage.Exists((string)outputPath))
            {
                return getTrackFromStorage(audioManager, resourceStore, outputPath);
            }

            try
            {
                string pathToFile = await convertFileAsync(Path.GetFileNameWithoutExtension(path), "mp3");
                await saveFileToStorageAsync(pathToFile, storage, outputPath);
                File.Delete(pathToFile);

                if (!storage.Exists(outputPath)) throw new FileNotFoundException("Audio not found");

                return getTrackFromStorage(audioManager, resourceStore, outputPath);
            }
            catch (FFMpegException)
            {
                throw;
            }
            catch (Exception ex)
            {
                handleException(ex, outputPath);
                return null;
            }
        }

        public async Task CreateTrackInStorageAsync(string path, Storage storage)
        {
            if (path == null)
            {
                Logger.Error(new ArgumentNullException(nameof(path)), "Path to video cannot be null");
                return;
            }

            var audioName = getHashName(path, "mp3");
            var videoName = getHashName(path, Path.GetExtension(path));

            if (storage.GetFiles(storage.GetFullPath(@"")).Contains(audioName))
            {
                Logger.Log("File exists in local storage");
                return;
            }

            try
            {
                string pathToAudio = await convertFileAsync(Path.GetFileNameWithoutExtension(path), "mp3");
                await saveFileToStorageAsync(pathToAudio, storage, audioName);
                await saveFileToStorageAsync(pathToAudio, storage, videoName);
                File.Delete(pathToAudio);
            }
            catch (FFMpegException f)
            {
                Logger.Error(f, "Video extraction error due to FFmpeg");
                throw;
            }
            catch (Exception ex)
            {
                handleException(ex, audioName);
                throw;
            }
        }

        private string getHashName(string path, string extension)
        {
            return $"{Hash.GetHashString(Path.GetFileNameWithoutExtension(path)).ToLower()}.{extension}";
        }

        private T getTrackFromStorage(AudioManager audioManager, IResourceStore<byte[]> resourceStore, string audioName)
        {
            return audioManager.GetTrackStore(resourceStore).Get(audioName) as T;
        }

        private async Task<string> convertFileAsync(string path, string extension, string outputPath = null,string arguments = null)
        {
            string pathToFile = await ConversionBase<AudioExtractModel>.Conversion(path, transferConfigManager, outputPath, arguments);
            if (!File.Exists(path))
            {
                Logger.Error(new Exception(), $"{path} does not exist - conversion canceled");
                throw new Exception($"{path} does not exist - conversion canceled");
            }
            return pathToFile;
        }

        private async Task saveFileToStorageAsync(string pathToFile, Storage storage, string name)
        {
            using (Stream file = new FileStream(pathToFile, FileMode.Open))
            {
                await saveStreamToStorageAsync(file, storage, name);
            }
        }

        private async Task saveStreamToStorageAsync(Stream file, Storage storage, string fileName)
        {
            using (var storageStream = storage.GetStream(fileName, FileAccess.Write, FileMode.OpenOrCreate))
            {
                Logger.Log($"File saved to {storage.GetFullPath(fileName)}");
                await file.CopyToAsync(storageStream);
            }
        }

        private void handleException(Exception ex, string audioName)
        {
            if (ex is FileNotFoundException) return;
            Logger.Error(ex, $"{GetType().Name} error");
        }




    }




}
