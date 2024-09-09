using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace Transfer.Game.UserInterface
{
    public partial class LoadingCircle : CompositeDrawable
    {
        private Circle circle;
        public LoadingCircle()
        {

            circle = new Circle
            {
                Size = new Vector2(25,25),
                Colour = Colour4.Transparent,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                BorderColour = Colour4.BlanchedAlmond,

            };
            InternalChild = circle;
        }



        protected override void LoadComplete()
        {
            circle.Loop(b => b.RotateTo(0).RotateTo(360,1000));
            base.LoadComplete();
        }

        protected override void Update()
        {
            base.Update();

        }
    }
}
