using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Graphics.UI.Containers
{
    public partial class WatchingToolsContainer : TransferFocusedOverlayContainer
    {
        public readonly VolumeContainer VolumeContainer;
        public readonly VideoPlaybackContainer PlaybackContainer;

        private readonly SpriteText currentPlaybackText;



        public WatchingToolsContainer()
        {
            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = [
                    VolumeContainer = new VolumeContainer
                    {
                        Width = 300,
                        Height = 10,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.BottomCentre,
                        Masking = true,
                        BorderThickness = 1f,
                        BorderColour = Colour4.Black,
                    },
                    new Container{
                        RelativeSizeAxes = Axes.Both,
                        Children = [
                            PlaybackContainer = new VideoPlaybackContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 10,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.BottomCentre,
                                Masking = true,
                                BorderThickness = 1f,
                                CornerRadius = 5,
                                BorderColour = Colour4.Black,
                            },
                            currentPlaybackText = new SpriteText{
                                Font = new FontUsage(family:TransferFonts.FiraCodeNerdFont),
                                Anchor = Anchor.Centre,
                                Origin = Anchor.BottomCentre,
                            }
                        ]
                    }

                ]
            };
            PlaybackContainer.SubscribeOnValueChange().ValueChanged += (ValueChangedEvent<double> value) => currentPlaybackText.Text = $"{DateTimeOffset.FromUnixTimeMilliseconds((long)value.NewValue).DateTime.ToString("HH:mm:ss")}/{DateTimeOffset.FromUnixTimeMilliseconds(duration).DateTime.ToString("HH:mm:ss")}";
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            VolumeContainer.Position = new Vector2(VolumeContainer.X - (VolumeContainer.Width / 3), -15);
            PlaybackContainer.SetSliderWidth(Width - VolumeContainer.SliderWidth * 4);
            PlaybackContainer.Position = new Vector2(PlaybackContainer.Position.X - VolumeContainer.SliderWidth / 2, -15);
            currentPlaybackText.Position = new Vector2(currentPlaybackText.Position.X + PlaybackContainer.Position.X / 2);
        }

        private long duration;
        public void SetMaxPlaybackValue(double value)
        {
            PlaybackContainer.SetMaxSliderValue(value);
            duration = (long)value;
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
