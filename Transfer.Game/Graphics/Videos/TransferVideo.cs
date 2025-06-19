using System;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Video;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Audio.Extensions;
using Transfer.Game.Configuration;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.Videos;

public partial class TransferVideo(string path, bool enableRate = false, bool startAtCurrentTime = true) : Video(path, startAtCurrentTime), IVideoController
{
    /// <summary>
    /// You can do animations when it occurs
    /// </summary>
    public Action<double> SeekOccurs;

    private readonly string path = path;

    private bool enableRate = enableRate;
    private bool isMuted = false;

    public bool IsPaused { get; private set; }

    [Resolved]
    private TempStorage tempStorage { get; set; }

    [NotNull]
    private AudioExtract<Track> audioExtract;

    private AudioManager audioManager;

    private readonly BindableFloat bindableRate = new BindableFloat()
    {
        Value = 1.0f
    };

    private TransferConfigManager transferConfigManager;

    [CanBeNull]
    protected Track Audio;

    public Action<bool> AudioLoading;

    protected override void Update()
    {
        base.Update();
        if (Audio != null) SpySeek(Audio.CurrentTime); // May be optimized in the future
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager transferConfigManager, AudioManager audioManager)
    {
        Logger.Log($"\ud83d\udcc2 Open output folder {tempStorage.GetFullPath("")}");
        this.audioManager = audioManager;
        audioExtract = new AudioExtract<Track>(transferConfigManager);
        this.transferConfigManager = transferConfigManager;
        bindableRate.Value = (float)transferConfigManager.Get<double>(TransferOptions.Rate);
    }

    private void rateChanged(ValueChangedEvent<float> obj)
    {
        if (Audio != null) Audio.Tempo.Value = obj.NewValue;
    }

    protected override async void LoadComplete()
    {
        base.LoadComplete();

        try
        {
            if (AudioExtractCoreExtension.ItContainsAudio(path))
            {
                Logger.Log("\ud83c\udfa7 Video have audio");
                Audio = await getAudio(tempStorage);
                if (Audio == null) throw new Exception("Audio extraction failed");
            }
            else Logger.Log("\ud83d\udd07 Video have not audio");

            if (Audio != null)
            {
                Logger.Log("Get from config value for Volume");
                Audio.Volume.ValueChanged += value =>
                {
                    transferConfigManager.GetBindable<double>(TransferOptions.Volume).Value = value.NewValue;
                };
            }

            bindableRate.ValueChanged += rateChanged;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Audio extraction failed. Try-catch error");
            throw;
        }

        Start();
    }

    private void onVolumeChanged(ValueChangedEvent<double> e)
    {
        if (Audio != null)
        {
            Audio.Volume.Value = (float)e.NewValue;
        }
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

    #region audio

    protected override bool OnScroll(ScrollEvent e)
    {
        if (Audio != null)
            Audio.Volume.Value += e.ScrollDelta.Y / 10;
        return base.OnScroll(e);
    }

    public void SetAudio(Track track) => Audio = track;

    private async Task<Track> getAudio(Storage audioStorage)
    {
        Logger.Log($"Preparing the conversion. Storage params {{ Path for storage file '{audioStorage.GetFullPath("")}' }} "
                   + $"{{ Path to temp file: '{path}' }}", LoggingTarget.Runtime, LogLevel.Debug);
        return await audioExtract.CreateaAndGetTrackAsync(path,
            audioStorage,
            audioManager,
            arguments: "-c copy"
        );
    }

    #endregion
}
