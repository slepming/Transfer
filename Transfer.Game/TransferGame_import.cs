using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Logging;
using Transfer.Game.IO;
using Transfer.Game.Screens;

namespace Transfer.Game;

public partial class TransferGame
{
    protected override void LoadComplete()
    {
        base.LoadComplete();

        dependencies.TryGet(out screenStack);
        dependencies.TryGet(out transferConfigManager);
        audioExtract = new AudioExtract<Track>(transferConfigManager);

        if (args is { Length: > 0 })
        {
            dependencies.TryGet(out tempResourceStore);
            dependencies.TryGet(out tempStorage);

            if (dependencies.TryGet(out AudioManager audioManager))
            {
                List<string> allowedPaths = new List<string>();

                foreach (string path in args)
                {
                    string fileName = Path.GetFileName(path);

                    if (!tempStorage.Exists(fileName) && !VIDEO_EXTENSIONS.Contains(Path.GetExtension(path)))
                        continue;

                    allowedPaths.Add(path);
                }

                lock (allowedPaths)
                    Import(allowedPaths.ToArray());

                allowedPaths.Clear();
                Scheduler.AddDelayed(() =>
                {
                    screenStack.Push(transferScreen = new VideoScreen(args[0]));
                }, 500);
            }
        }
        else
            screenStack.Push(transferScreen = new VideoScreen());
    }

    private void handleImportFromDrop()
    {
        lock (dropFiles)
        {
            Logger.Log($"Handling of {dropFiles.Count} files");
            string[] paths = dropFiles.ToArray();
            dropFiles.Clear();
            Task.Factory.StartNew(() =>
            {
                lock (dropFiles)
                {
                    Import(dropFiles.ToArray());
                }
            }, TaskCreationOptions.LongRunning);
        }
    }

    public Task Import(params string[] paths)
    {
        lock (paths)
        {
            if (paths.Length == 0) return Task.CompletedTask;

            if (paths.Length == 1)
            {
                audioExtract.CreateTrackInStorage(System.IO.Path.GetFullPath(paths[0]), tempStorage);
                return Task.CompletedTask;
            }

            foreach (string path in paths)
            {
                if (path == null) continue;

                if (!File.Exists(Path.GetFullPath(path)))
                {
                    Logger.Log($"File {Path.GetFileName(Path.GetFullPath(path))} is not exists");
                    continue;
                }

                Logger.Log(@$"""{Path.GetFileName(path)}"" been importing");
                audioExtract.CreateTrackInStorage(Path.GetFullPath(path), tempStorage);
            }

            return Task.CompletedTask;
        }
    }
}
