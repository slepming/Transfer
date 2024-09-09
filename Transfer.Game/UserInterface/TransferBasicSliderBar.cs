using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Vector2 = osuTK.Vector2;

namespace Transfer.Game.UserInterface
{
    public partial class TransferBasicSliderBar<T> : BasicSliderBar<T> where T : struct, INumber<T>, IMinMaxValue<T>
    {

        
        public TransferBasicSliderBar()
        {
            BackgroundColour = new Colour4(1,1,1,0.1f);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ScaleTo(new Vector2(1.1f, 1.1f), 200, Easing.Out);
            this.FadeColour(new Colour4(1,1,1,1f), 300, Easing.OutQuint);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            BackgroundColour = new Colour4(1,1,1,0.1f);
            this.FadeColour(new Colour4(1,1,1,0.5f), 150, Easing.OutQuint);
            this.ScaleTo(new Vector2(1,1), 100, Easing.In);
            base.OnHoverLost(e);
        }

        protected override void OnUserChange(T value)
        {
            Logger.Log("User change Slider bar");
            base.OnUserChange(value);
        }


    }
}