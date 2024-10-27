using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.UserInterface.Containers
{
    public partial class VolumeContainer : FocusedOverlayContainer
    {

        private TransferBasicSliderBar<double> volumeSlider;
        private BindableDouble sliderVolumeValue;

        public Action<ValueChangedEvent<double>> ValueChanged;

        public double MinValue = 0;
        public double MaxValue = 1;
        public double Value = 0.3d;


        public VolumeContainer()
        {
            Show();
            sliderVolumeValue = new BindableDouble
            {
                MinValue = MinValue,
                MaxValue = MaxValue,
                Value = Value
            };

            InternalChildren = new Drawable[]
            {
                volumeSlider = new TransferBasicSliderBar<double>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(200, 20),
                    TransferValueOnCommit = true,
                    KeyboardStep = 1,
                    SelectionColour = Color4.Pink,
                },
            };
            volumeSlider.Current.ValueChanged += ValueChanged;
            volumeSlider.Current = sliderVolumeValue;
        }
       
        protected override bool OnHover(HoverEvent e)
        {
            this.FadeColour(new Colour4(255, 255, 255, 0.5f), 300, Easing.OutQuint);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.FadeColour(Colour4.Transparent, 5000, Easing.OutQuint);
            base.OnHoverLost(e);
        }

        protected override void PopIn()
        {
            this.ScaleTo(new Vector2(1f, 1f), 200, Easing.InCubic);
        }

        protected override void PopOut()
        {
            this.ScaleTo(new Vector2(0.1f, 0.1f), 200, Easing.InOutCubic);
        }
    }
}
