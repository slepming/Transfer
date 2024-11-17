using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.UserInterface
{
    public partial class Loading : CompositeDrawable
    {
        private Box loadingComponent;
        private Container backgroundContainer;
        private Container loadingContainer;
        private Container fullComponentContainer;
        public Loading()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Container container = new Container()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Colour4.White,
                Children = new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Colour4.White,
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            backgroundContainer = new Container
                            {
                                Size = new Vector2(Width/2, Height/2),
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Masking = true,
                                EdgeEffect = new EdgeEffectParameters() {
                                    Radius = 400,
                                    Colour = Colour4.Gray,
                                    Type = EdgeEffectType.Shadow
                                }
                            },
                            loadingContainer = new Container
                            {
                                Size = new Vector2(Width/2, Height/2),
                                Masking = true,
                                MaskingSmoothness = 5f,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = 15f,
                                Child = loadingComponent = new Box
                                {
                                    Size = new Vector2(Width/2, Height/2),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    EdgeSmoothness = new Vector2(1,1),
                                },
                            }
                        }
                    }
                }
            };
            InternalChild = container;
        }

        protected override void LoadComplete()
        {

            base.LoadComplete();
            backgroundContainer.Loop(b => b.RotateTo(0).TransformTo(nameof(EdgeEffect), new EdgeEffectParameters() {
                                    Radius = 200,
                                    Colour = Colour4.White,
                                    Type = EdgeEffectType.Glow
                                }).RotateTo(-720,4000, Easing.InOutElastic).TransformTo(nameof(EdgeEffect), new EdgeEffectParameters() {
                                    Radius = 400,
                                    Colour = Colour4.Gray,
                                    Type = EdgeEffectType.Glow
                                }, 800, Easing.InOutQuad));
            loadingContainer.Loop(b => b.RotateTo(0)
            .TransformTo(nameof(Colour), ColourInfo.SingleColour(Color4.White))
            .RotateTo(720,4000, Easing.InOutElastic)
            .TransformTo(nameof(Colour),ColourInfo.SingleColour(Color4.Black), 800, Easing.InOutQuad));

        }
    }
}
