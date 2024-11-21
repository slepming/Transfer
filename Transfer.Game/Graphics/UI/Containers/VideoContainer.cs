using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
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


        private SpriteText mutedText;

        public Track Audio;

        private bool isMuted = false;

        private string filename;


        public VideoContainer(string filename)
        {
            this.filename = filename;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (string.IsNullOrWhiteSpace(filename)) Logger.Log("File path is null or file don't find");
            Logger.Log($"Video initialization");




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
                    Size = new Vector2(1, 1/6),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                },
            };

            video.SeekOccurs += onSeek;

            toolsContainer.Show();
            toolsContainer.VolumeContainer.Current.ValueChanged += onVolumeChanged;


            if (Audio != null)
            {
                Audio.Volume.ValueChanged += value => toolsContainer.VolumeContainer.DefaultValue = value.NewValue;
            }

        }


        private void onSeek(double obj)
        {
            return;
        }

        protected override void Update()
        {
            base.Update();


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
            Audio?.StartAsync();
        }


        private void onVolumeChanged(ValueChangedEvent<double> e)
        {
            if (Audio != null)
            {
                Audio.Volume.Value = (float)e.NewValue;
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
                    Audio?.Seek(video.PlaybackPosition);
                    break;
                case osuTK.Input.Key.Left:
                    video.Seek(video.PlaybackPosition - seekSpace);
                    Audio?.Seek(video.PlaybackPosition);
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
                        Audio?.Start();
                        video.IsPlaying = true;
                    }
                    else
                    {
                        Audio?.Stop();
                        video.IsPlaying = false;
                    }
                    isMuted = !isMuted;
                    return true;
                }
                case GlobalAction.WatchingVideoOnCurrentPlaybackRestart:
                {
                    if (Audio != null) Audio.RestartPoint = video.PlaybackPosition;
                    Audio?.Restart();
                    video.PlaybackPosition = Audio.RestartPoint;
                    return true;
                }
                case GlobalAction.WatchingVideoReset:
                {
                    Audio?.Restart();
                    video.PlaybackPosition = 0;
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
