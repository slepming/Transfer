#nullable disable
using System;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;

namespace Transfer.Game.Screens;

public partial class VideoScreen : TransferScreen, IKeyBindingHandler<GlobalAction>, ICanUpdateVideo
{
    private VideoContainer videoContainer;

    private FileSelectorDialog explorerContainer { get; set; }

    private WatchingToolsContainer toolsContainer;

    private ExtensionMenu extensionMenu;

    [Resolved]
    private AudioManager audioManager { get; set; }

    [Resolved]
    private FrameworkConfigManager frameworkConfigManager { get; set; }

    [Resolved]
    private TempStorage tempStorage { get; set; }

    [Resolved]
    private PlaylistStorage playlistStorage { get; set; }

    private PlaylistMenu cachePlaylist { get; set; }

    protected IAudioExtract<Track> AudioExtract { get; set; }

    private LoadingOverlay loadingOverlay = new LoadingOverlay() { Header = "Loading..." };

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
        seek = transferConfigManager.Get<int>(TransferOptions.SeekValue);
        cachePlaylist = new PlaylistMenu(playlistStorage)
        {
            Title = "Cache Playlist",
        };
        AddRangeInternal([explorerContainer ??= new FileSelectorDialog(this), extensionMenu ??= [], cachePlaylist]);
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

            case GlobalAction.OpenPlaylistMenu:
                cachePlaylist.Show();
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Releases resources in a way that does not affect the operation of the facility in question
    /// </summary>
    private void releaseResources()
    {
        videoContainer?.Dispose();
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }

    public void UpdateVideo(string path)
    {
        videoContainer = new VideoContainer(path);

        videoContainer.UpdateVideo(path);
        InternalChild = toolsContainer = new WatchingToolsContainer
        {
            RelativeSizeAxes = Axes.Both,
            Video = videoContainer,
        };
    }

    public void UpdateVideo(TransferVideo video)
    {
        videoContainer.UpdateVideo(video);
    }
}
