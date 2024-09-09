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
        Circle circle;
        public LoadingCircle()
        {


        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(circle = new Circle
            {
                Size = new Vector2(25,25),
                Colour = Colour4.AliceBlue,
                Anchor = Anchor.Centre,
                Origin = Anchor.TopLeft,
            });
        }

        protected override void LoadComplete()
        {

            base.LoadComplete();
        }
    }
}
