using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace KIO.Game.UserInterface.Buttons
{
    public partial class KIOBaseBackground : CompositeDrawable
    {
        private const int duration = 500;

        private readonly Box background;

        public KIOBaseBackground()
        {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 5;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Glow,
                Radius = 1.5f,
                Colour = Color4.Black.Opacity(0.5f), 
            };
            AddInternal(background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = new Color4(255,255,255, 0.1f)
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            
            


            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 10f,
                Colour = Color4.Black.Opacity(0.1f),
            }, duration, Easing.InOutQuad);


            base.OnHoverLost(e);
        }
    }
}