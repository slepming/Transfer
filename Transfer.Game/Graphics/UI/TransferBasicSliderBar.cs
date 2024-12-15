using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;
using Vector2 = osuTK.Vector2;

namespace Transfer.Game.UserInterface
{
    public partial class TransferBasicSliderBar<T> : BasicSliderBar<T> where T : struct, INumber<T>, IMinMaxValue<T>
    {

        
        public TransferBasicSliderBar()
        {
            BackgroundColour = new Colour4(255,255,255,1f);
        }

        protected override bool OnHover(HoverEvent e)
        {
            return base.OnHover(e);
        }



        protected override void OnUserChange(T value)
        {
            this.FlashColour(ColourInfo.GradientVertical(Color4.Pink, Color4.HotPink), 200, Easing.In);
        }


    }
}
