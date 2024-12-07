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

namespace Transfer.Game.Graphics.UI
{
    public partial class FinalMenu : Menu
    {
        public FinalMenu(Direction direction, bool topLevelMenu = false) : base(direction, topLevelMenu)
        {
            BackgroundColour = Colour4.Gray.Opacity(100);
            Width = 600;
            Height = 300;
            Size = new Vector2(0,0);
        }

        protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item) => new FinalDrawableMenuItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new BasicScrollContainer(direction);

        protected override Menu CreateSubMenu() => new FinalMenu(Direction.Vertical)
        {
            Anchor = Direction == Direction.Horizontal ? Anchor.BottomLeft : Anchor.TopRight
        };


        protected override void AnimateClose()
        {
            this.ScaleTo(0, 100, Easing.Out);
        }
        protected override void AnimateOpen()
        {
            this.ScaleTo(1, 100, Easing.In);
        }

        public partial class FinalDrawableMenuItem : DrawableMenuItem
        {
            public FinalDrawableMenuItem(MenuItem item) : base(item)
            {
                Masking = true;
                Content.Anchor = Anchor.Centre;
                Content.Origin = Anchor.Centre;
                Content.Colour = new Color4(255, 255, 255, 0.5f);
                
                RelativeSizeAxes = Axes.Both;
                BackgroundColour = Colour4.Gray.Opacity(100);
                BorderThickness = 2;
                BorderColour = Colour4.White.Opacity(150);
            }


            protected override bool OnHover(HoverEvent e)
            {
                Content.FadeColour(Colour4.White, 200, Easing.In);
                return false;
            }
            protected override void OnHoverLost(HoverLostEvent e)
            {
                Content.FadeColour(Colour4.White.Opacity(100), 200, Easing.Out);
            }

            protected override Drawable CreateContent() => new SpriteText
            {
                Font = new FontUsage("Oswald", size:30),
            };
        }
    }
}
