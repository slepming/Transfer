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
     public partial class MenuBackgroundButton : CompositeDrawable
    {
        private const int duration = 200;

        private readonly Box background;

        public MenuBackgroundButton()
        {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 5;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 1.5f,
                Colour = Color4.Black.Opacity(0.5f),
            };
            AddInternal(background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = new Color4(255,255,255, 100)
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 5f,
                Offset = new Vector2(0, 1),
                Colour = Color4.Black.Opacity(0f),
            }, duration, Easing.OutQuint);

            background.FadeColour(new Color4(255,255,255, 100), duration, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            TweenEdgeEffectTo(new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 1.5f,
                Colour = Color4.Black.Opacity(0.5f),
            }, duration, Easing.OutQuint);

            background.FadeColour(new Color4(255,255,255, 100), duration, Easing.OutQuint);

            base.OnHoverLost(e);
        }
    }
}