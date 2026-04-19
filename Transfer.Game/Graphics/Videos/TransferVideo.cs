using System;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Video;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.Videos;

public partial class TransferVideo(string path, bool enableRate = false, bool startAtCurrentTime = true) : Video(path, startAtCurrentTime), IVideoController
{
    /// <summary>
    /// Event when it occurs
    /// </summary>
    public Action<double> SeekOccurs;

    private Action<Track> onAudioExtract;

    private readonly string path = path;

    private bool enableRate = enableRate;
    private bool isMuted = false;

    public bool IsPaused { get; private set; }

    [Resolved]
    private TransferConfigManager transferConfigManager { get; set; }

    [Resolved]
    private FrameworkConfigManager frameworkConfigManager { get; set; }

    [Resolved]
    private AudioManager audioManager { get; set; }

    [Resolved]
    private TempStorage tempStorage { get; set; }

    [NotNull]
    private AudioExtract<Track> audioExtract;

    private readonly BindableFloat bindableRate = new BindableFloat()
    {
        Value = 1.0f
    };

    [CanBeNull]
    protected Track Audio;

    public Action<bool> AudioLoading;

    [BackgroundDependencyLoader]
    private void load()
    {
        Logger.Log($"\ud83d\udcc2 Open output folder {tempStorage.GetFullPath("")}", level: LogLevel.Debug);
        audioExtract = new AudioExtract<Track>(transferConfigManager);
        bindableRate.Value = (float)transferConfigManager.Get<double>(TransferOptions.Rate);
        onAudioExtract += preStart;
    }

    private void rateChanged(ValueChangedEvent<float> obj)
    {
        if (Audio != null) Audio.Tempo.Value = obj.NewValue;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Logger.Log($"Preparing the conversion. Storage params {{ Path for storage file '{tempStorage.GetFullPath("")}' }} "
                   + $"{{ Path to temp file: '{path}' }}", LoggingTarget.Runtime, LogLevel.Debug);

        Task.Run(() =>
        {
            var audio = audioExtract.CreateAndGetTrack(path,
                tempStorage,
                audioManager,
                arguments: "-c copy"
            );
            Scheduler.Add(() =>
            {
                onAudioExtract?.Invoke(audio);
            });
        });
    }

    private void preStart(Track track)
    {
        try
        {
            Audio = track;
            Audio.Volume.Value = frameworkConfigManager.Get<double>(FrameworkSetting.VolumeMusic);
            Logger.Log(frameworkConfigManager.Get<double>(FrameworkSetting.VolumeMusic).ToString(CultureInfo.CurrentCulture), level: LogLevel.Debug);
            Audio.Volume.ValueChanged += value =>
            {
                frameworkConfigManager.SetValue(FrameworkSetting.VolumeMusic, value.NewValue);
                frameworkConfigManager.Save();
            };

            bindableRate.ValueChanged += rateChanged;
        }
        catch(Exception e)
        {
            Logger.Error(e, "audio volume error(maybe video haven't audio)");
        }
        finally
        {
            Start();
        }

    }

    protected override void Update()
    {
        base.Update();
        if (Audio != null) SpySeek(Audio.CurrentTime); // May be optimized in the future
    }

    public double GetMaxLengthVideo()
    {
        return Duration;
    }

    public void Rate(float rate) => bindableRate.Value = rate;

    public void SpySeek(double time)
    {
        base.Seek(time);
    }

    public new void Seek(double time)
    {
        SeekOccurs?.Invoke(time);

        Audio?.Seek(time);
        base.Seek(time);
    }

    public void Start()
    {
        Logger.Log("Video started", level: LogLevel.Debug);
        Audio?.Start();
        IsPlaying = true;
    }

    public bool Pause()
    {
        Logger.Log("Video has been paused!", level: LogLevel.Debug);

        if (PlaybackPosition == Duration)
        {
            Audio?.Restart();
            Seek(0);
            return true;
        }

        if (isMuted)
        {
            Audio?.Start();
            IsPlaying = true;
        }
        else
        {
            Audio?.Stop();
            IsPlaying = false;
        }

        isMuted = !isMuted;
        IsPaused = !IsPaused;
        return true;
    }

    public bool RestartAudio()
    {
        if (Audio != null) Audio.RestartPoint = PlaybackPosition;
        Audio?.Restart();
        return true;
    }

    public bool Reset()
    {
        lock (this)
        {
            if (Audio != null) Audio.RestartPoint = 0;
            Audio?.Restart();
            Seek(0);
        }

        return true;
    }

    public void RemoveAudio()
    {
        Audio?.Dispose();
    }

    #region audio

    protected override bool OnScroll(ScrollEvent e)
    {
        if (Audio != null)
            Audio.Volume.Value += e.ScrollDelta.Y / 10;
        return base.OnScroll(e);
    }

    public void SetAudio(Track track) => Audio = track;

    #endregion

    protected override void Dispose(bool isDisposing)
    {
        Audio?.Dispose();
        base.Dispose(isDisposing);
    }
}
