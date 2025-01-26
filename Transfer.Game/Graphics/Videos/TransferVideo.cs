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
using Transfer.Game.Audio.Extensions;
using Transfer.Game.Configuration;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.Videos;

public partial class TransferVideo : Video, IKeyBindingHandler<GlobalAction>
{
    /// <summary>
    /// You can do animations when it occurs
    /// </summary>
    public Action<double> SeekOccurs;

    [Resolved]
    private Storage tempStorage { get; set; }

    private bool enableRate;

    private BindableFloat bindableRate = new BindableFloat()
    {
        Value = 1.0f
    };

    private string filename;

    [CanBeNull]
    private Track audio;

    [NotNull]
    private IAudioExtract<Track> audioExtract;

    private AudioManager audioManager;
    private bool isMuted = false;

    public TransferVideo(string filename, bool enableRate = false, bool startAtCurrentTime = true)
        : base(filename, startAtCurrentTime)
    {
        this.filename = filename;
        this.enableRate = enableRate;
    }

    protected override void Update()
    {
        base.Update();
        if (audio != null) SpySeek(audio.CurrentTime); // May be optimized in the future
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager transferConfigManager, AudioManager audioManager)
    {
        this.audioManager = audioManager;
        audioExtract = new AudioExtract<Track>(transferConfigManager);

        bindableRate.Value = (float)transferConfigManager.Get<double>(TransferOptions.Rate);

        if (AudioExtractCoreExtension.ItContainsAudio(filename))
        {
            Logger.Log("Video have audio");
            Task.Run(async () =>
            {
                audio = await getAudio(tempStorage);
            });
        }
        else Logger.Log("Video have not audio");

        if (audio != null)
        {
            audio.Volume.ValueChanged += value =>
            {
                transferConfigManager.GetBindable<double>(TransferOptions.Volume).Value = value.NewValue;
            };
        }

        bindableRate.ValueChanged += rateChanged;
    }

    private void rateChanged(ValueChangedEvent<float> obj)
    {
        if (audio != null) audio.Tempo.Value = obj.NewValue;
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
                    audio?.Restart();
                    Seek(0);
                    return true;
                }

                if (isMuted)
                {
                    audio?.Start();
                    IsPlaying = true;
                }
                else
                {
                    audio?.Stop();
                    IsPlaying = false;
                }

                isMuted = !isMuted;
                return true;
            }

            case GlobalAction.WatchingVideoOnCurrentPlaybackRestart:
            {
                if (audio != null) audio.RestartPoint = PlaybackPosition;
                audio?.Restart();
                return true;
            }

            case GlobalAction.WatchingVideoReset:
            {
                lock (this)
                {
                    if (audio != null) audio.RestartPoint = 0;
                    audio?.Restart();
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
        if (audio != null)
            audio.Volume.Value += e.ScrollDelta.Y / 10;
        return base.OnScroll(e);
    }

    public void SetAudio(Track track) => audio = track;

    private Task<Track> getAudio(Storage audioStorage)
    {
        return audioExtract.CreateaAndGetTrackAsync(filename, audioStorage, audioManager);
    }

    #endregion
}
