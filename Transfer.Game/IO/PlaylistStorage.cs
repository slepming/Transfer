using System;
using System.IO;
using System.Threading.Tasks;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Configuration;

namespace Transfer.Game.IO;

public class PlaylistStorage : WrappedStorage
{
    private PlaylistConfigManager configuration;

    public PlaylistStorage(Storage underlyingStorage, string subPath = null)
        : base(underlyingStorage, subPath)
    {
        createConfig(underlyingStorage);
    }

    private async Task saveDataFromPath(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            using (var storageStream = UnderlyingStorage.GetStream(path, FileAccess.Write, FileMode.OpenOrCreate))
            {
                await fileStream.CopyToAsync(storageStream);
            }
        }
    }

    private void createConfig(Storage storage)
    {
        configuration = new PlaylistConfigManager(storage);
        configuration.SetValue(PlaylistConfiguration.Path, storage.GetFullPath(""));
        configuration.SetValue(PlaylistConfiguration.Name, Path.GetDirectoryName(storage.GetFullPath("")));
    }

    [Obsolete("In this case, it will cause a lot of latency, so I strongly suggest using references rather than moving objects around completely")]
    public async Task SaveDataFromPathAsync(string path) => await saveDataFromPath(path);

    private void saveDataFromPathToLink(string path)
    {
        var info = File.CreateSymbolicLink(path, UnderlyingStorage.GetFullPath(""));
    }

    public void SaveDataFromPathToLink(string path) => saveDataFromPathToLink(path);

    private string getFileNameFromLink(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        return Path.GetFileName(fileInfo.FullName);
    }

    public string GetFileNameFromLink(string path) => getFileNameFromLink(path);

    private void deleteData(string fileName)
    {
        UnderlyingStorage.Delete(fileName);
    }

    /// <summary>
    /// Delete file from storage
    /// </summary>
    /// <param name="fileName">File name if your playlist is directories use path to file</param>
    public void DeleteData(string fileName) => deleteData(fileName);
}
