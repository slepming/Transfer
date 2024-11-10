using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.UserInterface
{
    public partial class TransferSpacingButton : ClickableContainer
    {
        public string Text;
        public Action ButtonAction;
        public Action OnDoubleClickAction;
        public FontUsage Font = new FontUsage(size: 30);

        public TransferSpacingButton()
        {
            AddRange(new Drawable[]{


            });
            Masking = true;
            Action = () => ButtonAction?.Invoke();

        }


        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new SpacingText{
                    Font = Font,
                    Text = Text,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Colour4.WhiteSmoke
                });
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            OnDoubleClickAction?.Invoke();
            return base.OnDoubleClick(e);
        }

        protected override bool OnClick(ClickEvent e)
        {

            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.FadeColour(new Colour4(255,255,255, 0.1f), 200, Easing.InOutQuad);
            return base.OnHover(e);

        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.FadeColour(new Colour4(255,255,255, 1f), 200, Easing.InOutQuad);
            base.OnHoverLost(e);
        }
    }
}
