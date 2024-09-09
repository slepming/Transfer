using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.UserInterface.Containers
{
    public partial class VolumeContainer : OverlayContainer, IDisposable
    {

        private TransferBasicSliderBar<double> volumeSlider;
        private BindableDouble sliderVolumeValue;

        public Action<ValueChangedEvent<double>> ValueChanged;

        public double MinValue = 0;
        public double MaxValue = 1;
        public double Value = 0.5d;


        public VolumeContainer()
        {

        }
        protected override void LoadComplete()
        {
            base.LoadComplete();
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
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    Position = new Vector2(0, -40),
                    Size = new Vector2(200, 20),
                    TransferValueOnCommit = true,
                    KeyboardStep = 1,
                    SelectionColour = Color4.Pink,
                },
            };
            volumeSlider.Current.ValueChanged += ValueChanged;
            volumeSlider.Current = sliderVolumeValue;
        }

        protected override void Dispose(bool isDisposing)
        {
            volumeSlider?.Dispose();
            base.Dispose(isDisposing);
        }

        protected override void PopIn()
        {

        }

        protected override void PopOut()
        {

        }
    }
}
