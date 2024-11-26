using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Graphics.UI.Containers
{
    public partial class WatchingToolsContainer : TransferFocusedOverlayContainer
    {
        public readonly VolumeContainer VolumeContainer;
        public readonly VideoPlaybackContainer PlaybackContainer;
        public bool VolumeSliderVisible { get; private set; } = true;

        [Resolved]
        private GameHost host { get; set; }


        public WatchingToolsContainer()
        {
            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = [
                    VolumeContainer = new VolumeContainer
                    {
                        Width = 300,
                        Height = 20,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.BottomCentre,
                        Masking = true,
                        BorderThickness = 1f,
                        BorderColour = Colour4.Black,
                    },
                    PlaybackContainer = new VideoPlaybackContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 20,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.BottomCentre,
                        Masking = true,
                        BorderThickness = 1f,
                        BorderColour = Colour4.Black,
                    }

                ]
            };

        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            VolumeContainer.Position = new Vector2(VolumeContainer.X - (VolumeContainer.Width / 3), -15);
            PlaybackContainer.SetSliderWidth(host.Window.ClientSize.Width - VolumeContainer.SliderWidth * 4);
            PlaybackContainer.Position = new Vector2(PlaybackContainer.Position.X - VolumeContainer.SliderWidth / 2, -15);
        }

        public void SetMaxPlaybackValue(double value)
        {
            PlaybackContainer.SetMaxSliderValue(value);
        }


        public void SetPlaybackValueForSlider(double value)
        {
            PlaybackContainer.SetSliderBarValue(value);
        }

        protected override void PopIn()
        {
            this.FadeTo(1, 1000, Easing.InOutQuad);
        }

        protected override void PopOut()
        {
            this.FadeTo(0, 0);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.FadeTo(1, 100, Easing.InOutQuad);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.FadeTo(0.001f, 2000, Easing.InOutQuad);
            base.OnHoverLost(e);
        }
    }
}
