using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI
{
    public partial class ConfirmButton : ClickableContainer
    {
        private Box background;
        private SpriteText buttonText;

        public ConfirmButton()
        {
            Masking = true;
            CornerRadius = 10;
            Colour = Colour4.White;
            InternalChildren =
            [
                background = new Box{
                    Alpha = 0.5f,
                    Colour = Colour4.FromHex("#0df8f5"),
                    RelativeSizeAxes = Axes.Both,
                },
                buttonText = new SpriteText{
                    Text = "Confirm",
                    Font = new FontUsage(TransferFonts.Oswald),
                    Colour = Colour4.DarkBlue,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            ];
        }



        protected override bool OnHover(HoverEvent e)
        {
            this.FadeColour(Colour4.FromHex("#155653"), 200, Easing.OutQuart);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.FadeColour(Colour4.White, 250);
            base.OnHoverLost(e);
        }

    }
}
