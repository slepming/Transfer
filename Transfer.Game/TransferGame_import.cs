using System.IO;
using System.Threading.Tasks;
using osu.Framework.Logging;

namespace Transfer.Game;

public partial class TransferGame
{
    private void handleImportFromDrop()
    {
        lock (dropFiles)
        {
            Logger.Log($"Handling of {dropFiles.Count} files");
            string[] paths = dropFiles.ToArray();
            dropFiles.Clear();
            Task.Factory.StartNew(() => Import(dropFiles.ToArray()), TaskCreationOptions.LongRunning);
        }
    }

    public Task Import(params string[] paths)
    {
        lock (paths)
        {
            if (paths.Length == 0) return Task.CompletedTask;

            if (paths.Length == 1)
                audioExtract.CreateTrackInStorageAsync(System.IO.Path.GetFullPath(paths[0]), tempStorage);

            foreach (string path in paths)
            {
                if (path == null) continue;

                if (!File.Exists(Path.GetFullPath(path)))
                {
                    Logger.Log($"File {Path.GetFileName(Path.GetFullPath(path))} is not exists");
                    continue;
                }

                Logger.Log(@$"""{Path.GetFileName(path)}"" been importing");
                audioExtract.CreateTrackInStorageAsync(Path.GetFullPath(path), tempStorage);
            }

            return Task.CompletedTask;
        }
    }
}
