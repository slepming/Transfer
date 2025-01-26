#nullable disable
using System;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Screens;

public partial class VideoScreen : TransferScreen, IKeyBindingHandler<GlobalAction>
{
    private VideoContainer videoContainer;

    private ExplorerContainer explorerContainer { get; set; } = new ExplorerContainer();

    private WatchingToolsContainer toolsContainer;

    private ExtensionMenu extensionMenu;

    [Resolved]
    private AudioManager audioManager { get; set; }

    [Resolved]
    private FrameworkConfigManager frameworkConfigManager { get; set; }

    [Resolved]
    private Storage tempStorage { get; set; }

    protected IAudioExtract<Track> AudioExtract { get; set; }

    private int seek;

    private string videoPath { get; set; }

    public string VideoPath
    {
        get => videoPath;
        set
        {
            if (videoPath == value) return;

            videoPath = value;
        }
    }

    public VideoScreen() { }
    public VideoScreen(string pathToVideo) => VideoPath = pathToVideo;

    public VideoScreen(Track audio, string pathToVideo)
    {
        VideoPath = pathToVideo;
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager transferConfigManager)
    {
        AudioExtract = new AudioExtract<Track>(transferConfigManager);
        explorerContainer.FoundVideo += onFoundVideo;
        seek = transferConfigManager.Get<int>(TransferOptions.SeekValue);
        extensionMenu = new ExtensionMenu();
        AddInternal((explorerContainer ??= []));
        AddInternal(extensionMenu = new ExtensionMenu());
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        if (string.IsNullOrEmpty(VideoPath))
        {
            const string nullable = "Video null";
            Logger.Log(nullable);
            explorerContainer.Show();
            return;
        }

        try
        {
            videoContainer = new VideoContainer(videoPath)
            {
                RelativeSizeAxes = Axes.Both,
                SeekSpace = seek
            };
            //frameworkConfigManager?.SetValue(FrameworkSetting.WindowedSize, videoContainer is not null ? new Size((int)videoContainer.Video.Size.X, (int)videoContainer.Video.Size.Y) : new Size(1280, 720));
            InternalChild = toolsContainer = new WatchingToolsContainer
            {
                RelativeSizeAxes = Axes.Both,
                Video = videoContainer,
            };
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Unhandled exception: ");
            AddInternal(new ExceptionContainer
            {
                HeaderText = "Error",
                Text = ex.Message,
                RelativeSizeAxes = Axes.Both,
            });
        }
    }

    private async void onFoundVideo(string path, bool isFile)
    {
        try
        {
            if (!isFile) return;

            LoadingOverlay loading;
            Task<Track> trackTask = AudioExtract.CreateaAndGetTrackAsync(path, tempStorage, audioManager: audioManager);
            ClearInternal();
            AddInternal(loading = new LoadingOverlay()
            {
                Header = "Uploading a video"
            });
            Track track = await trackTask ?? null;
            loading.Expire();

            this.Push(new VideoScreen(track, path));
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Unhandled exception: ");
            ClearInternal();
            AddInternal(new ExceptionContainer
            {
                HeaderText = "Fatal error",
                Text = ex.Message,
                RelativeSizeAxes = Axes.Both
            });
        }
    }

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.OpenEditor:

                return true;

            case GlobalAction.Explorer:
                releaseResources();
                this.Push(new VideoScreen());
                return true;

            case GlobalAction.ExtensionMenu:
                if (extensionMenu.Visible)
                    extensionMenu.Show();
                else
                    extensionMenu.Hide();
                return true;
        }

        return false;
    }

    /// <summary>
    /// Releases resources in a way that does not affect the operation of the facility in question
    /// </summary>
    private void releaseResources()
    {
        videoContainer?.Dispose();
        explorerContainer?.Dispose();
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }
}
