using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Threading;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.Screens;

namespace Transfer.Game;

public partial class TransferGame : TransferGameBase, IKeyBindingHandler<GlobalAction>, ICanAcceptFile
{
    public const int DIALOGS_DEPTH = 3000;
    private Screen transferScreen;

    private readonly string[] args = [];

    private TempStorage tempStorage = null;

    private StorageBackedResourceStore tempResourceStore = null;

    private ScreenStack screenStack = null;

    private IAudioExtract<Track> audioExtract;
    private DependencyContainer dependencies;
    private TransferConfigManager transferConfigManager;
    private PlaylistMenu playlistMenu;
    private AssemblyInfoDialog assemblyInfoDialog;

    [Resolved]
    private PlaylistStorage historyPlaylistsStorage { get; set; }

    public TransferGame(string[] args)
    {
        this.args = args;
    }

    public TransferGame() { }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        AddRange([
            assemblyInfoDialog = new AssemblyInfoDialog()
            {
                Depth = DIALOGS_DEPTH
            },
            playlistMenu = new PlaylistMenu(historyPlaylistsStorage)
            {
                Depth = DIALOGS_DEPTH
            }
        ]);

        dependencies.CacheAs(playlistMenu);

        dependencies.TryGet(out screenStack);
        dependencies.TryGet(out transferConfigManager);
        audioExtract = new AudioExtract<Track>(transferConfigManager);

        try
        {
            if (args.Length > 0)
            {
                dependencies.TryGet(out tempResourceStore);
                dependencies.TryGet(out tempStorage);

                if (dependencies.TryGet(out AudioManager audioManager))
                {
                    List<string> allowedPaths = new List<string>();

                    foreach (string path in args)
                    {
                        if (!File.Exists(path))
                            continue;

                        string fileName = Path.GetFileName(path);

                        if (!tempStorage.Exists(fileName) && !VIDEO_EXTENSIONS.Contains(Path.GetExtension(path)))
                            continue;

                        allowedPaths.Add(path);
                    }

                    lock (allowedPaths)
                        Import(allowedPaths.ToArray());

                    allowedPaths.Clear();
                    Scheduler.AddDelayed(async void () =>
                    {
                        if (await audioManager.GetTrackStore(tempResourceStore).GetAsync($"{Hash.GetHashString(Path.GetFileNameWithoutExtension(args[0]))}.mp3") is Track audio)
                            screenStack.Push(transferScreen = new VideoScreen(audio, pathToVideo: args[0]));
                        else
                            screenStack.Push(transferScreen = new VideoScreen(pathToVideo: args[0], audio: null));
                    }, 500);
                }
            }
            else
                screenStack.Push(transferScreen = new VideoScreen());
        }
        catch
        {
            throw;
        }
    }

    private readonly List<string> dropFiles = new List<string>();
    private ScheduledDelegate dropScheduledDelegate;

    public override void SetHost(GameHost host)
    {
        base.SetHost(host);

        if (host.Window != null)
        {
            host.Window.DragDrop += path =>
            {
                if (VIDEO_EXTENSIONS.Contains(Path.GetExtension(path)))
                {
                    lock (dropFiles)
                    {
                        dropFiles.Add(path);
                        Logger.Log(@$"File ""{System.IO.Path.GetFileName(path)}"" was imported");

                        dropScheduledDelegate?.Cancel();
                        dropScheduledDelegate = Scheduler.AddDelayed(handleImportFromDrop, 100);
                    }
                }
                else
                    Logger.Log("Unhandled extension");
            };
        }
    }

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

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

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

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.OpenAssemblyVersion:
                assemblyInfoDialog.Show();
                return true;

            case GlobalAction.OpenPlaylistMenu:
                playlistMenu.Show();
                return true;
        }

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }
}
