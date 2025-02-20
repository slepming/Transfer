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
    private readonly TransferConfigManager transferConfigManager1;

    private static readonly string tempFolder = Path.Combine(Path.GetTempPath(), "Transfer");
    private readonly Storage tempStorage;

    public AudioExtract(TransferConfigManager transferConfigManager)
    {
        transferConfigManager1 = transferConfigManager ?? throw new ArgumentNullException(nameof(transferConfigManager));
        if (!Directory.Exists(tempFolder))
            Directory.CreateDirectory(tempFolder);
        tempStorage = new NativeStorage(tempFolder);
    }

    public async Task<T> CreateaAndGetTrackAsync(string path, Storage storage, AudioManager audioManager, string outputPath = null, string arguments = null)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (audioManager == null) throw new ArgumentNullException(nameof(audioManager));

        outputPath ??= getHashName(path, "mp3");
        var resourceStore = new StorageBackedResourceStore(storage);

        if (storage.Exists(outputPath))
        {
            return getTrackFromStorage(audioManager, resourceStore, outputPath);
        }

        if (File.Exists(Path.Combine(tempFolder, outputPath)))
        {
            return getTrackFromStorage(audioManager, new StorageBackedResourceStore(tempStorage), outputPath);
        }

        try
        {
            string pathToFile = await convertFileAsync(path, "mp3", storage.GetFullPath("")) + ".mp3";
            Logger.Log($"Path to file: {pathToFile}\n output path: {outputPath}");
            await saveFileToStorageAsync(pathToFile, storage, outputPath);
            File.Delete(pathToFile);

            if (!storage.Exists(outputPath))
            {
                throw new FileNotFoundException("Audio not found");
            }

            return await CreateaAndGetTrackAsync(path, storage, audioManager, outputPath);
        }
        catch (FFMpegException)
        {
            throw;
        }
        catch (Exception ex)
        {
            handleException(ex, outputPath);
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

    private async Task<string> convertFileAsync(string path, string extension, string outputPath = null, string arguments = null)
    {
        Logger.Log($"Path is null: {path is null}({path})\n Extension is null: {extension is null}\n Output is null: {outputPath is null}({outputPath})\n Arguments: {arguments is null}");
        outputPath ??= Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{extension}");

        if (!File.Exists(path))
        {
            Logger.Error(new Exception(), $"{path} does not exist - conversion canceled");
            throw new Exception($"{path} does not exist - conversion canceled");
        }

        string pathToFile = await ConversionBase<AudioExtractModel>.Conversion(path, transferConfigManager1, outputPath, arguments);

        return pathToFile;
    }

    private async Task saveFileToStorageAsync(string paToFile, Storage storage, string name)
    {
        Logger.Log($"Path to file is null: {paToFile}\n Name is null: {name}");

        using (Stream file = new FileStream(paToFile, FileMode.Open))
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
