using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Video;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Audio;
using Transfer.Game.Audio.Extensions;
using Transfer.Game.Configuration;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.Videos;

public partial class TransferVideo(string filename, bool enableRate = false, bool startAtCurrentTime = true) : Video(filename, startAtCurrentTime), IKeyBindingHandler<GlobalAction>
{
    /// <summary>
    /// You can do animations when it occurs
    /// </summary>
    public Action<double> SeekOccurs;

    [Resolved]
    private TempStorage tempStorage { get; set; }

    private bool enableRate = enableRate;

    private BindableFloat bindableRate = new BindableFloat()
    {
        Value = 1.0f
    };

    private string filename = filename;

    private TransferConfigManager transferConfigManager;

    [CanBeNull]
    protected Track Audio;

    [NotNull]
    private IAudioExtract<Track> audioExtract;

    private AudioManager audioManager;
    private bool isMuted = false;

    protected override void Update()
    {
        base.Update();
        if (Audio != null) SpySeek(Audio.CurrentTime); // May be optimized in the future
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager transferConfigManager, AudioManager audioManager)
    {
        Logger.Log(@$"Open file {filename}");
        Logger.Log($"Open folder {tempStorage.GetFullPath("")}");
        this.audioManager = audioManager;
        audioExtract = new AudioExtract<Track>(transferConfigManager);
        this.transferConfigManager = transferConfigManager;
        bindableRate.Value = (float)transferConfigManager.Get<double>(TransferOptions.Rate);
    }

    private void rateChanged(ValueChangedEvent<float> obj)
    {
        if (Audio != null) Audio.Tempo.Value = obj.NewValue;
    }

    protected override async void LoadAsyncComplete()
    {
        base.LoadAsyncComplete();

        if (AudioExtractCoreExtension.ItContainsAudio(filename))
        {
            Logger.Log("Video have audio");
            Audio = await getAudio(tempStorage);
        }
        else Logger.Log("Video have not audio");

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

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        switch (e.Action)
        {
            case GlobalAction.PauseVideo:
            {
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
                return true;
            }

            case GlobalAction.WatchingVideoOnCurrentPlaybackRestart:
            {
                if (Audio != null) Audio.RestartPoint = PlaybackPosition;
                Audio?.Restart();
                return true;
            }

            case GlobalAction.WatchingVideoReset:
            {
                lock (this)
                {
                    if (Audio != null) Audio.RestartPoint = 0;
                    Audio?.Restart();
                    Seek(0);
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
        return await audioExtract.CreateaAndGetTrackAsync(filename, audioStorage, audioManager);
    }

    #endregion
}
