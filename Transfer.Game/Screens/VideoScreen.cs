#nullable disable
using System;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;

namespace Transfer.Game.Screens;

public partial class VideoScreen : TransferScreen, IKeyBindingHandler<GlobalAction>, ICanUpdateVideo
{
    private VideoContainer videoContainer;

    public FileSelectorDialog FileSelector { get; private set; }

    private WatchingToolsContainer toolsContainer;

    private readonly ExtensionMenu extensionMenu = new();

    [Resolved]
    private AudioManager audioManager { get; set; }

    [Resolved]
    private FrameworkConfigManager frameworkConfigManager { get; set; }

    [Resolved]
    private TempStorage tempStorage { get; set; }

    [Resolved]
    private PlaylistStorage playlistStorage { get; set; }

    private PlaylistMenu cachePlaylist { get; set; }

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

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager transferConfigManager)
    {
        seek = transferConfigManager.Get<int>(TransferOptions.SeekValue);
        cachePlaylist = new PlaylistMenu(playlistStorage, this)
        {
            Title = "Cache Playlist",
            OnHandledException = Exception
        };
        AddRangeInternal([FileSelector ??= new FileSelectorDialog(this), extensionMenu, cachePlaylist]);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        if (string.IsNullOrEmpty(VideoPath))
        {
            const string nullable = "Video null";
            Logger.Log(nullable);
            FileSelector.Show();
            return;
        }

        try
        {
            UpdateVideo(videoPath);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Unhandled exception: ");
            Exception(ex);
        }
    }

    protected override Box CreateBackground() => new()
    {
        Colour = Colour4.FromHex("#010221")
    };

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.OpenEditor:
                return true;

            case GlobalAction.Explorer:
                FileSelector.Show();
                return true;

            case GlobalAction.ExtensionMenu:
                if (extensionMenu.State.Value == Visibility.Visible)
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

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }

    public void UpdateVideo(string path)
    {
        Logger.Log($"Update from {GetType()}");
        videoContainer ??= new VideoContainer(path)
        {
            SeekSpace = seek
        };

        videoContainer.UpdateVideo(path);

        if (toolsContainer == null)
        {
            AddRangeInternal(
                [
                    videoContainer,
                    toolsContainer = new WatchingToolsContainer
                    {
                        Video = videoContainer,
                    },
                ]
            );
        }
    }

    public void UpdateVideo(TransferVideo video)
    {
        videoContainer.UpdateVideo(video);
    }
}
