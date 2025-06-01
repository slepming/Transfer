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

namespace Transfer.Game.IO;

public class AudioExtract<T> : IAudioExtract<T>
    where T : Track
{
    private readonly TransferConfigManager transferConfigManager;

    private string tempFolder => Path.Combine(Path.GetTempPath(), "Transfer");
    private readonly Storage tempStorage;

    public AudioExtract(TransferConfigManager transferConfigManager)
    {
        this.transferConfigManager = transferConfigManager ?? throw new ArgumentNullException(nameof(transferConfigManager));
        if (!Directory.Exists(tempFolder))
            Directory.CreateDirectory(tempFolder);
        tempStorage = new NativeStorage(tempFolder);
    }

    public async Task<T> CreateaAndGetTrackAsync(string path, Storage storage, AudioManager audioManager, string outputName = null, string arguments = null)
    {
        if (!File.Exists(path))
        {
            Logger.Error(new Exception(), $"{path} does not exist - conversion canceled");
            throw new Exception($"{path} does not exist - conversion canceled");
        }

        if (audioManager == null) throw new ArgumentNullException(nameof(audioManager));

        outputName ??= getHashName(path, "mp3");
        Logger.Log($"The final output file name '{outputName}'", level: LogLevel.Debug);
        var resourceStore = new StorageBackedResourceStore(storage);

        if (storage.Exists(outputName))
        {
            Logger.Log("File existed in local storage, move to getTrackFromStorage", level: LogLevel.Debug);
            return getTrackFromStorage(audioManager, resourceStore, outputName);
        }

        if (File.Exists(Path.Combine(tempFolder, outputName)))
        {
            Logger.Log("File existed in temp files, move to getTrackFromStorage", level: LogLevel.Debug);
            return getTrackFromStorage(audioManager, new StorageBackedResourceStore(tempStorage), outputName);
        }

        try
        {
            string pathToFile = await convertFileAsync(path);
            Logger.Log($"Path to file: {pathToFile}\n output path: {storage.GetFullPath("")}");
            await saveFileToStorageAsync(pathToFile, storage, outputName);
            File.Delete(pathToFile);

            /*if (!storage.Exists(outputPath))
            {
                throw new FileNotFoundException("Audio not found");
            }*/

            return getTrackFromStorage(audioManager, resourceStore, outputName) ?? throw new Exception("Failed to get track");
        }
        catch (FFMpegException)
        {
            throw;
        }
        catch (Exception ex)
        {
            handleException(ex, outputName);
            throw;
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
            string pathToAudio = await convertFileAsync(Path.GetFileNameWithoutExtension(path));
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

    private async Task<string> convertFileAsync(string path, string outputPath = null, string arguments = null)
    {
        outputPath ??= tempFolder;

        string pathToFile = await ConversionBase.Conversion<AudioExtractModel>(path,
            transferConfigManager,
            new FileParamsBuilder()
                .SetPath(outputPath)
                .SetBitrate(transferConfigManager.Get<int>(TransferOptions.AudioBitrate))
                .SetAudioCodec(transferConfigManager.Get<AudioCodecs>(TransferOptions.AudioCodec))
                .SetFileName(Path.GetFileNameWithoutExtension(path)), arguments);

        return pathToFile;
    }

    private async Task saveFileToStorageAsync(string pathToFile, Storage storage, string name)
    {
        Logger.Log("Save file to storage");

        using (Stream file = new FileStream(pathToFile, FileMode.Open))
        {
            await saveStreamToStorageAsync(file, storage, name);
        }
    }

    private async Task saveStreamToStorageAsync(Stream file, Storage storage, string fileName)
    {
        try
        {
            using (var storageStream = storage.GetStream(fileName, FileAccess.Write, FileMode.OpenOrCreate))
            {
                Logger.Log($"File saved to {storage.GetFullPath(fileName)}");
                await file.CopyToAsync(storageStream);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Logger.Log("Access denied. Connecting to another Storage");

            using (var storageStream = tempStorage.GetStream(fileName, FileAccess.Write, FileMode.OpenOrCreate))
            {
                Logger.Log($"File saved to {storage.GetFullPath(fileName)}");
                await file.CopyToAsync(storageStream);
            }
        }
    }

    private void handleException(Exception ex, string audioName)
    {
        if (ex is FileNotFoundException) return;

        Logger.Error(ex, $"{GetType().Name} error");
    }
}
