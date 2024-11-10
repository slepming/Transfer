using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics.UI
{
    public partial class DisappearingIconButton : IconButton
    {
        protected override bool OnHover(HoverEvent e)
        {
            this.TransformTo(nameof(Alpha), 1, 600, Easing.InOutQuad);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.TransformTo(nameof(Alpha), 0, 2000, Easing.InOutQuad);
            base.OnHoverLost(e);
        }
    }
}
