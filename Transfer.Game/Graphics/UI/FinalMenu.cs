using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using Transfer.Game.UserInterface.DirectoryHandler;

namespace Transfer.Game.Graphics.UI
{
    public partial class FinalMenu : Menu
    {
        public FinalMenu(Direction direction, bool topLevelMenu = false) : base(direction, topLevelMenu)
        {
            BackgroundColour = Colour4.Transparent;
        }

        protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item) => new FinalDrawableMenuItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new BasicScrollContainer(direction);

        protected override Menu CreateSubMenu() => new FinalMenu(Direction.Vertical)
        {
            Anchor = Direction == Direction.Horizontal ? Anchor.BottomLeft : Anchor.TopRight
        };

        protected override void AnimateClose()
        {
            this.ScaleTo(0, 200, Easing.InOutQuad);
            base.AnimateClose();
        }
        protected override void AnimateOpen()
        {
            this.ScaleTo(1, 200, Easing.InOutQuad);
            base.AnimateOpen();
        }

        public partial class FinalDrawableMenuItem : DrawableMenuItem
        {
            public FinalDrawableMenuItem(MenuItem item) : base(item)
            {
                BackgroundColour = FrameworkColour.Blue;
                CornerRadius = 4;
                Masking = true;

                RelativeSizeAxes = Axes.Both;
                Colour = Colour4.White;
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(Colour4.Aqua, 100, Easing.In);
                return false;
            }
            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeColour(Colour4.White, 100, Easing.In);
            }

            protected override Drawable CreateContent() => new SpriteText
            {
                Font = new FontUsage("FiraCodeNerdFont"),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
        }
    }
}
