using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.UserInterface.Containers
{
    public partial class VideoContainer : Container, IKeyBindingHandler<GlobalAction>
    {
        private TransferVideo video;

        private WatchingToolsContainer toolsContainer;
        private Container mediaOptionsContainer;

        private Bindable<double> bindableRate = new Bindable<double>();



        private SpriteText mutedText;

        private Track audio;

        public Track Audio { set
            {
                if (value == audio) return;
                audio = value;
            } }

        private bool isMuted = false;

        private string filename;


        public VideoContainer(string filename)
        {
            this.filename = filename;
        }

        [BackgroundDependencyLoader]
        private void load(TransferConfigManager tcm)
        {
            if (string.IsNullOrWhiteSpace(filename)) Logger.Log("File path is null or file don't find");
            Logger.Log($"Video initialization");

            defaultRate = tcm.Get<double>(TransferOptions.Rate);
            bindableRate.Value = defaultRate;

            InternalChildren = new Drawable[]
            {
                video = new TransferVideo(filename)
                {
                    FillMode = FillMode.Fit,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Loop = false,
                },
                toolsContainer = new WatchingToolsContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                },
            };

            video.SeekOccurs += onSeek;

            toolsContainer.Show();
            toolsContainer.VolumeContainer.Current.ValueChanged += onVolumeChanged;

            toolsContainer.VolumeContainer.SetSliderValue(tcm.Get<double>(TransferOptions.Volume));

            if (audio != null)
            {
                audio.Volume.ValueChanged += value =>
                {
                    toolsContainer.VolumeContainer.SetSliderValue(value.NewValue);
                    tcm.GetBindable<double>(TransferOptions.Volume).Value = value.NewValue;
                };
            }

            bindableRate.ValueChanged += onRateChanged;

        }

        private void onRateChanged(ValueChangedEvent<double> obj)
        {
            audio.Tempo.Value = obj.NewValue;
        }

        private void onSeek(double obj)
        {
            return;
        }

        protected override void Update()
        {
            base.Update();
            video.PlaybackPosition = audio.CurrentTime;
            toolsContainer.PlaybackContainer.SetSliderBarValue(video.PlaybackPosition);
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
            toolsContainer.SetMaxPlaybackValue(video.Duration);
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

        private double defaultRate;
        protected override bool OnKeyDown(KeyDownEvent e)
        {

            switch (e.Key)
            {
                case osuTK.Input.Key.Up:
                    toolsContainer.VolumeContainer.ChangeSliderValue(0.2, '+');
                    break;
                case osuTK.Input.Key.Down:
                    toolsContainer.VolumeContainer.ChangeSliderValue(0.2, '-');
                    break;
                case osuTK.Input.Key.Right:
                    video.Seek(video.PlaybackPosition + seekSpace);
                    audio?.Seek(video.PlaybackPosition);
                    break;
                case osuTK.Input.Key.Left:
                    video.Seek(video.PlaybackPosition - seekSpace);
                    audio?.Seek(video.PlaybackPosition);
                    break;
                case osuTK.Input.Key.Number0:
                    bindableRate.Value = defaultRate;
                    break;
                case osuTK.Input.Key.Number1:
                    bindableRate.Value = 1.0;
                    break;
                case osuTK.Input.Key.Number2:
                    bindableRate.Value = 2.0;
                    break;
                case var key when key == osuTK.Input.Key.LControl && key == osuTK.Input.Key.Number2:
                    bindableRate.Value += 2.0;
                    break;

            }
            return base.OnKeyDown(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            toolsContainer.VolumeContainer.ChangeSliderValue(e.ScrollDelta.Y / 10, '+');
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
                    video.PlaybackPosition = audio.RestartPoint;
                    return true;
                }
                case GlobalAction.WatchingVideoReset:
                {
                    bindableRate.Value = defaultRate;
                    lock (video)
                    {
                        Task.Run(() =>
                        {
                            audio?.Restart();
                            video.Seek(0);
                        });
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

    }
}
