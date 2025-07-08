using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using Transfer.Game.Audio;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.Cursor;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;

namespace Transfer.Game;

[Cached]
public partial class TransferGameBase : osu.Framework.Game
{
    public static readonly string[] VIDEO_EXTENSIONS = [".mp4", ".webm"];
#if DEBUG
    public const string HOST_NAME = "Transfer(Development)";
#else
    public const string HOST_NAME = "Transfer";
#endif

    // Anything in this class is shared between the test browser and the game implementation.
    // It allows for caching global dependencies that should be accessible to tests, or changing
    // the screen scaling for all components including the test browser and framework overlays.
    protected override Container<Drawable> Content { get; }

    private FontStore fontStore;

    protected Storage Storage;
    private TempStorage tempStorage;
    private PlaylistStorage historyPlaylistsStorage;

    protected SafeAreaContainer SafeAreaContainer;

    protected GlobalActionContainer GlobalBindings;

    private ScreenStack screenStack;
    private TransferConfigManager transferConfigManager;

    private DependencyContainer dependencies;

    private int allowableExceptions;

    protected TransferGameBase()
    {
        base.Content.Add(Content = new DrawSizePreservingFillContainer
        {
            TargetDrawSize = new Vector2(1024, 720)
        });
    }

    [BackgroundDependencyLoader]
    private void load(FrameworkConfigManager config, IRenderer renderer)
    {
        TransferFFmpegCore.CONVERSION_STATUS.ValueChanged += loggerFFmpeg;
        Host.Window.Title = HOST_NAME;
        tempStorage = new TempStorage(Storage.GetStorageForDirectory(@"./Temp/"));
        dependencies.CacheAs(tempStorage);
        Resources.AddStore(new DllResourceStore(@"Transfer.Resources.dll"));

        transferConfigManager = new TransferConfigManager(Host.Storage);
        dependencies.Cache(transferConfigManager);
        string pathToPlaylist = transferConfigManager.Get<string>(TransferOptions.HistoryPlaylistStoragePath);
        if (!Path.Exists(pathToPlaylist))
            Directory.CreateDirectory(pathToPlaylist);
        historyPlaylistsStorage = new PlaylistStorage(new NativeStorage(pathToPlaylist));
        dependencies.Cache(historyPlaylistsStorage);

        IResourceStore<byte[]> tempResourceStore = new StorageBackedResourceStore(tempStorage);
        dependencies.Cache(tempResourceStore);

        fontStore = new FontStore(renderer, null, 100f);
        Fonts.AddStore(fontStore);
        Resources.Get("Fonts/");

        InitialiseFonts();
        InitialiseConfig(config);

        Host.Window.CursorState = CursorState.Hidden;

        base.Content.Add(SafeAreaContainer = new SafeAreaContainer
        {
            RelativeSizeAxes = Axes.Both,
            SafeAreaOverrideEdges = Edges.None,
            Child = CreateScalingContainer().WithChild(GlobalBindings = new GlobalActionContainer(this)
            {
                Children =
                [
                    screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both },
                    new TransferCursorContainer
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                ]
            })
        });

        dependencies.Cache(screenStack);
        dependencies.Cache(GlobalBindings);
    }

    private void loggerFFmpeg(ValueChangedEvent<string> obj)
    {
        Logger.Log(obj.NewValue);
    }

    protected DrawSizePreservingFillContainer CreateScalingContainer() => new DrawSizePreservingFillContainer();

    public override void SetHost(GameHost host)
    {
        base.SetHost(host);
        Storage = host.Storage;
        host.ExceptionThrown += onExceptionThrown;
    }

    private bool onExceptionThrown(Exception exception)
    {
        if (Interlocked.Decrement(ref allowableExceptions) < 0)
        {
            Logger.Log("Too many unhandled exceptions, crashing out.");
            return false;
        }

        Logger.Log($"Unhandled exception has been allowed with {allowableExceptions} more allowable exceptions.");
        Task.Delay(1000).ContinueWith(_ => Interlocked.Increment(ref allowableExceptions));

        return true;
    }

    protected virtual void InitialiseFonts()
    {
        AddFont(Resources, @"Fonts/FiraCode/FiraCodeNerdFont");
        AddFont(Resources, @"Fonts/FiraCode/FiraCodeNerdFont-Light");
        AddFont(Resources, @"Fonts/FiraCode/FiraCodeNerdFont-Bold");

        AddFont(Resources, @"Fonts/Oswald/Oswald");
    }

    protected virtual void InitialiseConfig(FrameworkConfigManager config)
    {
        config.GetBindable<FrameSync>(FrameworkSetting.FrameSync).Value = FrameSync.VSync;
        config.GetBindable<WindowMode>(FrameworkSetting.WindowMode).Value = WindowMode.Windowed;
        config.GetBindable<RendererType>(FrameworkSetting.Renderer).Value = RendererType.Automatic;
        config.GetBindable<double>(FrameworkSetting.VolumeMusic).Value = 1d;
        config.Save();
    }

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
        dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        fontStore.Dispose();
        TransferFFmpegCore.CONVERSION_STATUS.ValueChanged -= loggerFFmpeg;
    }

    protected override bool OnExiting()
    {
        transferConfigManager.Save();
        return base.OnExiting();
    }
}
