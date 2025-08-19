using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.Playlists;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class VideoContainer : TransferContainer, IKeyBindingHandler<GlobalAction>, ICanUpdateVideo, ICanSaveFileToPlaylist
{
    private IVideoController videoController;

    private readonly LoadingOverlay loadingOverlay = new LoadingOverlay();

    [Resolved]
    private PlaylistStorage playlistStorage { get; set; }

    public double DefaultRate { get; private set; }

    public bool VideoIsPaused => videoController.IsPaused;

    public bool VideoLooping { get; private set; }

    public VideoContainer(string filename)
    {
        UpdateVideo(filename);
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager tcm)
    {
        Logger.Log("\u21ba Video initialization", level: LogLevel.Debug);
        RelativeSizeAxes = Axes.Both;

        DefaultRate = tcm.Get<double>(TransferOptions.Rate);
        VideoLooping = tcm.Get<bool>(TransferOptions.LoopVideo);
    }

    private void audioLoading(bool obj)
    {
        if (obj)
        {
            loadingOverlay.Show();
            Logger.Log("Start loading", level: LogLevel.Debug);
        }
        else
        {
            loadingOverlay.Hide();
            Logger.Log("Stop loading", level: LogLevel.Debug);
        }
    }

    public double GetDuration() => videoController.Duration;

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case osuTK.Input.Key.Right:
                allSeek(videoController.PlaybackPosition + seekSpace);
                break;

            case osuTK.Input.Key.Left:
                allSeek(videoController.PlaybackPosition - seekSpace);
                break;

            case osuTK.Input.Key.Number1:
                allSeek(videoController.GetMaxLengthVideo() * 0.1);
                break;

            case osuTK.Input.Key.Number2:
                allSeek(videoController.GetMaxLengthVideo() * 0.2);
                break;

            case osuTK.Input.Key.Number3:
                allSeek(videoController.GetMaxLengthVideo() * 0.3);
                break;

            case osuTK.Input.Key.Number4:
                allSeek(videoController.GetMaxLengthVideo() * 0.4);
                break;

            case osuTK.Input.Key.Number5:
                allSeek(videoController.GetMaxLengthVideo() * 0.5);
                break;

            case osuTK.Input.Key.Number6:
                allSeek(videoController.GetMaxLengthVideo() * 0.6);
                break;

            case osuTK.Input.Key.Number7:
                allSeek(videoController.GetMaxLengthVideo() * 0.7);
                break;

            case osuTK.Input.Key.Number8:
                allSeek(videoController.GetMaxLengthVideo() * 0.8);
                break;

            case osuTK.Input.Key.Number9:
                allSeek(videoController.GetMaxLengthVideo() * 0.9);
                break;
        }

        return base.OnKeyDown(e);
    }

    private double seekSpace = 1000;

    public double SeekSpace
    {
        get => seekSpace;
        set
        {
            if (seekSpace == value) return;

            seekSpace = Math.Abs(value);
        }
    }

    private void allSeek(double time)
    {
        videoController?.Seek(time);
    }

    private void onSeek(double obj)
    {
    }

    /// <summary>
    /// Seek video(in ms)
    /// </summary>
    /// <param name="time">ms for seeking</param>
    public void Seek(double time) => videoController.Seek(time);

    public void SetVideoLoop(bool loop) => videoController.Loop = loop;

    public void Rate(float rate)
    {
        if (rate < 0.5f) throw new ArgumentException(nameof(rate) + " Can't be less than 0.5");

        videoController.Rate(rate);
    }

    public bool Pause()
    {
        return videoController.Pause();
    }

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.PauseVideo:
            {
                return Pause();
            }

            case GlobalAction.WatchingVideoOnCurrentPlaybackRestart:
            {
                return videoController.RestartAudio();
            }

            case GlobalAction.WatchingVideoReset:
            {
                return videoController.Reset();
            }

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }

    public void SaveFileToPlaylist(string pathToFile)
    {
        playlistStorage?.SaveDataFromPathToLink(pathToFile);
    }

    public void UpdateVideo(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new NullReferenceException("Path to video is null.");

        var video = new TransferVideo(path)
        {
            FillMode = FillMode.Fit,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Loop = VideoLooping,
        };
        InternalChild = video;
        video.SeekOccurs += onSeek;
        videoController = video;
        SaveFileToPlaylist(path);
        Logger.Log("\u2705 Video update confirm");
    }

    public void UpdateVideo(TransferVideo video = null)
    {
        this.videoController = video;
    }
}
