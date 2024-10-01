using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Transfer.Game.UserInterface
{
    public partial class Loading : CompositeDrawable
    {
        private Circle circleLoadingComponent;
        private Vector2 size = new Vector2(20,20);
        public Loading()
        {


        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Container container = new Container()
            {
                Masking = true,
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
                        Child = circleLoadingComponent = new Circle
                        {
                            Size = size,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,
                            BorderColour = Colour4.Black,
                            BorderThickness = 2,

                        }
                    }
                }
            };
            InternalChild = container;
        }

        protected override void LoadComplete()
        {

            base.LoadComplete();
            circleLoadingComponent.Loop(b => b.RotateTo(0).RotateTo(720,2000, Easing.OutBounce));

        }
    }
}
