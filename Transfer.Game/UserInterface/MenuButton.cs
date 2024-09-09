using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace KIO.Game.UserInterface.Buttons
{
    public abstract partial class MenuButton : ClickableContainer
    {
        protected readonly Box Background;

        public MenuButton(string text, Action action)
        {
            AddRange(new Drawable[]
            {
                new MenuBackgroundButton(),
                new SpriteText{
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = text
                },
            });
            Action = () => action?.Invoke();
        }

        protected override bool OnHover(HoverEvent e)
        {
            base.OnHover(e);
            return true;
        }
    }
}
