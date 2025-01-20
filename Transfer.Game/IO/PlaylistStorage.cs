using System.IO; using System.Threading.Tasks;
using osu.Framework.Platform;

namespace Transfer.Game.IO;

public class PlaylistStorage : WrappedStorage
{
    public PlaylistStorage(Storage underlyingStorage, string subPath = null)
        : base(underlyingStorage, subPath)
    {
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

    public async Task SaveDataFromPathAsync(string path) => await saveDataFromPath(path);

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
