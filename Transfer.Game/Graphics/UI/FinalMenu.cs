using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
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
            this.FadeColour(Colour4.Transparent, 200, Easing.InOutQuad);
            base.AnimateClose();
        }
        protected override void AnimateOpen()
        {
            this.FadeColour(Colour4.White, 200, Easing.InOutQuad);
            base.AnimateOpen();
        }

        public partial class FinalDrawableMenuItem : DrawableMenuItem
        {
            public FinalDrawableMenuItem(MenuItem item) : base(item)
            {
                Masking = true;
                Content.Width = 125;
                Content.Height = 25;
                Content.Anchor = Anchor.Centre;
                Content.Origin = Anchor.Centre;
                Content.Colour = new Color4(255, 255, 255, 1f);
                BackgroundColour = Colour4.Transparent;
            }


            protected override bool OnHover(HoverEvent e)
            {
                this.TransformTo(nameof(Content.Colour), ColourInfo.SingleColour(new Color4(255, 255, 255, 1f)), 400, Easing.InOutQuad);
                return false;
            }
            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.TransformTo(nameof(Content.Colour), ColourInfo.SingleColour(new Color4(255, 255, 255, 0.5f)), 200, Easing.InOutQuad);
            }

            protected override Drawable CreateContent() => new SpriteText
            {
                Font = new FontUsage("Oswald", size:30),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
        }
    }
}
