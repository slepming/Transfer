using System;
using System.Numerics;
using KIO.Game.UserInterface.Buttons;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace KIO.Game.UserInterface
{
    public partial class TransferButton : ClickableContainer
    {
        public string Text;
        public Action ButtonAction;
        public TransferButton(string text, Action action)
        {
            AddRange(new Drawable[]{
                new KIOBaseBackground(),
                new SpriteText{
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = text
                },
            });
            Action = () => action?.Invoke();
        }
        public TransferButton(string text)
        {
            AddRange(new Drawable[]{
                new KIOBaseBackground(),
                new SpriteText{
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = text
                },
            });
        }
        public TransferButton()
        {
            AddRange(new Drawable[]{
                new KIOBaseBackground(),
                new SpriteText{
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = this.Text
                },
            });
            Action = () => ButtonAction?.Invoke();
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.FadeColour(new Colour4(255,255,255, 0.1f), 200, Easing.InQuad);
            this.ScaleTo(1.2f, 200, Easing.InQuad);
            return base.OnHover(e);
            
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            
            base.OnHoverLost(e);
        }
    }
}
