using System.Collections.Generic;
using System.IO;
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

    private void createConfig(Storage storage)
    {
        configuration = new PlaylistConfigManager(storage);
        configuration.SetValue(PlaylistConfiguration.Path, storage.GetFullPath(""));
        configuration.SetValue(PlaylistConfiguration.Name, Path.GetDirectoryName(storage.GetFullPath("")));
    }

    private void saveDataFromPathToLink(string path)
    {
        if (UnderlyingStorage.Exists(Path.GetFileName(path)))
            UnderlyingStorage.Delete(Path.GetFileName(path));

        string targetLinkPath = Path.Combine(GetFullPath(string.Empty), Path.GetFileName(Path.GetFullPath(path)));
        string fileForLinkPath = Path.GetFullPath(path);

        Logger.Log($"Create symbolic link from {fileForLinkPath} to {targetLinkPath}", level: LogLevel.Debug);
        if (File.Exists(targetLinkPath))
            return;

        File.CreateSymbolicLink(targetLinkPath, fileForLinkPath);
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
    public override void Delete(string fileName) => deleteData(fileName);

    private void deleteAllData(string path = "", string pattern = "*")
    {
        IEnumerable<string> files = UnderlyingStorage.GetFiles(path, pattern);

        foreach (string file in files)
        {
            deleteData(file);
        }
    }

    public void DeleteAll(string path = "", string pattern = "*")
    {
        deleteAllData(MutatePath(path), pattern);
    }
}
