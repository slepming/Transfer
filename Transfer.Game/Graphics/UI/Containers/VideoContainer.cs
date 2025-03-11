using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class VideoContainer : Container, IKeyBindingHandler<GlobalAction>
{
    public TransferVideo Video { get; private set; }

    private readonly LoadingOverlay loadingOverlay = new LoadingOverlay();

    private readonly string filename;

    public double DefaultRate { get; private set; }

    public VideoContainer(TransferVideo transferVideo) => Video = transferVideo;

    public VideoContainer(string filename)
    {
        this.filename = filename;
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager tcm)
    {
        if (string.IsNullOrWhiteSpace(filename)) Logger.Log("File path is null or file don't find");
        Logger.Log($"Video initialization");

        DefaultRate = tcm.Get<double>(TransferOptions.Rate);
        if (Video == null) Logger.Log("Video equal null. Assigning the values given to me");
        Video ??= new TransferVideo(filename)
        {
            FillMode = FillMode.Fit,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Loop = false,
        };
        Video.AudioLoading += AudioLoading;
        Video.SeekOccurs += onSeek;
        Add(Video);
    }

    private void AudioLoading(bool obj)
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

    public double GetDuration() => Video.Duration;

    public string GetTimecode() => Video == null ? null : $"{DateTimeOffset.FromUnixTimeMilliseconds((long)Video.PlaybackPosition).DateTime:HH:mm:ss}/{DateTimeOffset.FromUnixTimeMilliseconds((long)Video.Duration).DateTime:HH:mm:ss}";

    public Vector2 GetVideoSize() => new Vector2(Video.Width, Video.Height);

    protected override void LoadComplete()
    {
        if (Video.IsFaulted)
        {
            AddInternal(new ExceptionContainer
            {
                HeaderText = "Error",
                Text = "Decoder error",
                RelativeSizeAxes = Axes.Both,
            });
        }

        Video.Start();

        base.LoadComplete();
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case osuTK.Input.Key.Right:
                allSeek(Video.PlaybackPosition + seekSpace);
                break;

            case osuTK.Input.Key.Left:
                allSeek(Video.PlaybackPosition - seekSpace);
                break;

            case osuTK.Input.Key.Number1:
                allSeek(Video.GetMaxLengthVideo() * 0.1);
                break;

            case osuTK.Input.Key.Number2:
                allSeek(Video.GetMaxLengthVideo() * 0.2);
                break;

            case osuTK.Input.Key.Number3:
                allSeek(Video.GetMaxLengthVideo() * 0.3);
                break;

            case osuTK.Input.Key.Number4:
                allSeek(Video.GetMaxLengthVideo() * 0.4);
                break;

            case osuTK.Input.Key.Number5:
                allSeek(Video.GetMaxLengthVideo() * 0.5);
                break;

            case osuTK.Input.Key.Number6:
                allSeek(Video.GetMaxLengthVideo() * 0.6);
                break;

            case osuTK.Input.Key.Number7:
                allSeek(Video.GetMaxLengthVideo() * 0.7);
                break;

            case osuTK.Input.Key.Number8:
                allSeek(Video.GetMaxLengthVideo() * 0.8);
                break;

            case osuTK.Input.Key.Number9:
                allSeek(Video.GetMaxLengthVideo() * 0.9);
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

            seekSpace = value;
        }
    }

    private void allSeek(double time)
    {
        Video?.Seek(time);
    }

    private void onSeek(double obj)
    {
    }

    /// <summary>
    /// Seek video(in ms)
    /// </summary>
    /// <param name="time">ms for seeking</param>
    public void Seek(double time) => Video.Seek(time);

    public void SetVideoLoop(bool loop) => Video.Loop = loop;

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        return true;
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }

    public void UpdateVideo(string path, [CanBeNull] TransferVideo video = null) => Video = video ?? new TransferVideo(path)
    {
        FillMode = FillMode.Fit,
        RelativeSizeAxes = Axes.Both,
        Anchor = Anchor.Centre,
        Origin = Anchor.Centre,
        Loop = false,
    };

    protected override void Dispose(bool isDisposing)
    {
        Video.Expire();
        base.Dispose(isDisposing);
    }
}
