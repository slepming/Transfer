using System;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.UserInterface.Containers;

public partial class VideoContainer : Container, IKeyBindingHandler<GlobalAction>
{
    private TransferVideo video;
    private Container mediaOptionsContainer;

    private Bindable<double> bindableRate = new Bindable<double>();

    private Track audio;

    public Track Audio
    {
        set
        {
            if (value != null && value == audio) return;

            audio = value;
        }
    }

    private bool isMuted = false;

    private string filename;
    private string timeCode;

    public double DefaultRate { get; private set; }

    public VideoContainer(TransferVideo transferVideo) => video = transferVideo;

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
        bindableRate.Value = DefaultRate;

        video ??= new TransferVideo(filename)
        {
            FillMode = FillMode.Fit,
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Loop = false,
        };
        Add(video);

        video.SeekOccurs += onSeek;

        if (audio != null)
        {
            audio.Volume.ValueChanged += value =>
            {
                tcm.GetBindable<double>(TransferOptions.Volume).Value = value.NewValue;
            };
        }

        bindableRate.ValueChanged += onRateChanged;
    }

    public double GetDuration() => video.Duration;

    public string GetTimecode() => video == null ? null : $"{DateTimeOffset.FromUnixTimeMilliseconds((long)video.PlaybackPosition).DateTime:HH:mm:ss}/{DateTimeOffset.FromUnixTimeMilliseconds((long)video.Duration).DateTime:HH:mm:ss}";

    public Vector2 GetVideoSize() => new Vector2(video.Width, video.Height);

    private void onRateChanged(ValueChangedEvent<double> obj)
    {
        if (audio != null) audio.Tempo.Value = obj.NewValue;
    }

    protected override void Update()
    {
        base.Update();
        if (audio != null) video.SpySeek(audio.CurrentTime);
    }

    protected override void LoadComplete()
    {
        if (video.IsFaulted)
        {
            AddInternal(new ExceptionContainer
            {
                HeaderText = "Error",
                Text = "Decoder error",
                RelativeSizeAxes = Axes.Both,
            });
        }

        base.LoadComplete();
    }

    protected override void LoadAsyncComplete()
    {
        base.LoadAsyncComplete();
        audio?.StartAsync();
    }

    private void onVolumeChanged(ValueChangedEvent<double> e)
    {
        if (audio != null)
        {
            audio.Volume.Value = (float)e.NewValue;
        }
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

            seekSpace = value;
        }
    }

    private void allSeek(double time)
    {
        video?.Seek(time);
        audio?.Seek(time);
    }

    private void onSeek(double obj)
    {
    }

    public void Seek(double time) => allSeek(time);

    public void Rate(double rate) => bindableRate.Value = rate;

    protected override bool OnScroll(ScrollEvent e)
    {
        if (audio != null)
            audio.Volume.Value += e.ScrollDelta.Y / 10;
        return base.OnScroll(e);
    }

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.PauseVideo:
            {
                if(video.PlaybackPosition == video.Duration)
                {
                    audio?.Restart();
                    video.Seek(0);
                    return true;
                }

                if (isMuted)
                {
                    audio?.Start();
                    video.IsPlaying = true;
                }
                else
                {
                    audio?.Stop();
                    video.IsPlaying = false;
                }

                isMuted = !isMuted;
                return true;
            }

            case GlobalAction.WatchingVideoOnCurrentPlaybackRestart:
            {
                if (audio != null) audio.RestartPoint = video.PlaybackPosition;
                audio?.Restart();
                return true;
            }

            case GlobalAction.WatchingVideoReset:
            {
                lock (video)
                {
                    if(audio != null) audio.RestartPoint = 0;
                    audio?.Restart();
                    video.Seek(0);
                }

                return true;
            }

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }

    protected override void Dispose(bool isDisposing)
    {
        audio?.Dispose();
        video.Expire();
        base.Dispose(isDisposing);
    }
}
