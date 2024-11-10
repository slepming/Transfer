using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Graphics.UI.Containers
{
    public partial class WatchingToolsContainer : TransferFocusedOverlayContainer
    {
        public readonly VolumeContainer VolumeContainer;
        public bool VolumeSliderVisible { get; private set; } = true;

        public WatchingToolsContainer()
        {
            InternalChild = new Container{
                RelativeSizeAxes = Axes.Both,
                Children = [
                    VolumeContainer = new VolumeContainer
                    {
                        Width = 300,
                        Height = 20,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.BottomCentre,
                    },
                ]
            };
            VolumeContainer.Position = new Vector2(VolumeContainer.X - (VolumeContainer.Width/3), -15);
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
            this.FadeTo(1,100, Easing.InOutQuad);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.FadeTo(0.001f, 2000, Easing.InOutQuad);
            base.OnHoverLost(e);
        }
    }
}
