using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.Playlists;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class VideoContainer : TransferContainer, IKeyBindingHandler<GlobalAction>, ICanUpdateVideo, ICanSaveFileToPlaylist
{
    private TransferVideo video;

    private readonly LoadingOverlay loadingOverlay = new LoadingOverlay();

    private readonly string filename;

    [Resolved]
    private PlaylistStorage playlistStorage { get; set; }

    public double DefaultRate { get; private set; }

    public bool VideoIsPaused => video.IsPaused;

    public VideoContainer(TransferVideo transferVideo) => video = transferVideo;

    public VideoContainer(string filename)
    {
        this.filename = filename;
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager tcm)
    {
        if (string.IsNullOrWhiteSpace(filename)) Logger.Log("File path is null or file don't find");
        Logger.Log("\u21ba Video initialization", level: LogLevel.Debug);
        RelativeSizeAxes = Axes.Both;

        DefaultRate = tcm.Get<double>(TransferOptions.Rate);
        if (video == null) Logger.Log("Constructor Video equal null. Assigning the values given to me", level: LogLevel.Debug);
        video ??= new TransferVideo(filename)
        {
            FillMode = FillMode.Fit,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Loop = false,
        };
        if (filename != null)
            SaveFileToPlaylist(filename);
        video.AudioLoading += audioLoading;
        video.SeekOccurs += onSeek;
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

    public double GetDuration() => video.Duration;

    public string GetTimecode() => video == null ? null : $"{DateTimeOffset.FromUnixTimeMilliseconds((long)video.PlaybackPosition).DateTime:HH:mm:ss}/{DateTimeOffset.FromUnixTimeMilliseconds((long)video.Duration).DateTime:HH:mm:ss}";

    public Vector2 GetVideoSize() => new Vector2(video.Width, video.Height);

    protected override void LoadComplete()
    {
        base.LoadComplete();

        if (video.IsFaulted)
        {
            AddInternal(new ErrorDialog()
            {
                Title = "Error",
                Message = "Decoder error",
                RelativeSizeAxes = Axes.Both,
            });
            return;
        }

        InternalChild = video;
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case osuTK.Input.Key.Right:
                allSeek(video.PlaybackPosition + seekSpace);
                break;

            case osuTK.Input.Key.Left:
                allSeek(video.PlaybackPosition - seekSpace);
                break;

            case osuTK.Input.Key.Number1:
                allSeek(video.GetMaxLengthVideo() * 0.1);
                break;

            case osuTK.Input.Key.Number2:
                allSeek(video.GetMaxLengthVideo() * 0.2);
                break;

            case osuTK.Input.Key.Number3:
                allSeek(video.GetMaxLengthVideo() * 0.3);
                break;

            case osuTK.Input.Key.Number4:
                allSeek(video.GetMaxLengthVideo() * 0.4);
                break;

            case osuTK.Input.Key.Number5:
                allSeek(video.GetMaxLengthVideo() * 0.5);
                break;

            case osuTK.Input.Key.Number6:
                allSeek(video.GetMaxLengthVideo() * 0.6);
                break;

            case osuTK.Input.Key.Number7:
                allSeek(video.GetMaxLengthVideo() * 0.7);
                break;

            case osuTK.Input.Key.Number8:
                allSeek(video.GetMaxLengthVideo() * 0.8);
                break;

            case osuTK.Input.Key.Number9:
                allSeek(video.GetMaxLengthVideo() * 0.9);
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
        video?.Seek(time);
    }

    private void onSeek(double obj)
    {
    }

    /// <summary>
    /// Seek video(in ms)
    /// </summary>
    /// <param name="time">ms for seeking</param>
    public void Seek(double time) => video.Seek(time);

    public void SetVideoLoop(bool loop) => video.Loop = loop;

    public void Rate(float rate)
    {
        if (rate < 0.5f) throw new ArgumentException(nameof(rate) + " Can't be less than 0.5");

        video.Rate(rate);
    }

    public bool Pause()
    {
        return video.Pause();
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
                return video.RestartAudio();
            }

            case GlobalAction.WatchingVideoReset:
            {
                return video.Reset();
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
        if (string.IsNullOrEmpty(path)) throw new NullReferenceException("Path to video is null.");

        video?.RemoveAudio();

        video = new TransferVideo(path)
        {
            FillMode = FillMode.Fit,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Loop = false,
        };
        SaveFileToPlaylist(path);
        Logger.Log("\u2705 Video update confirm");
    }

    public void UpdateVideo(TransferVideo video = null)
    {
        this.video?.RemoveAudio();
        this.video = video;
    }
}
