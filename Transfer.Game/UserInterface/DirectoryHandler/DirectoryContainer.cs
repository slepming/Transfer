using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public partial class DirectoryContainer : FocusedOverlayContainer
    {

        public DirectoryContainer()
        {
            Logger.Log("Create Directory Container");
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {

        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ScaleTo(1f,600,Easing.InOutQuad);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ScaleTo(0.5f, 400, Easing.InOutQuad);
            base.OnHoverLost(e);
        }

        protected override void PopIn()
        {
            this.ScaleTo(0.5f,2000, Easing.InOutElastic);
        }

        protected override void PopOut()
        {
            this.ScaleTo(0.0f, 2000, Easing.InOutElastic);
        }

    }
}
