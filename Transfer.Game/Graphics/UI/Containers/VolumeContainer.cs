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
using Transfer.Game.Graphics.UI.Containers;

namespace Transfer.Game.UserInterface.Containers
{
    public partial class VolumeContainer : Container
    {

        private TransferBasicSliderBar<double> volumeSlider;
        private BindableDouble sliderVolumeValue;

        public Bindable<double> Current = new Bindable<double>();

        public double MinValue = 0;
        public double MaxValue = 1;
        public double DefaultValue = 0.3d;


        public VolumeContainer()
        {

            sliderVolumeValue = new BindableDouble
            {
                MinValue = MinValue,
                MaxValue = MaxValue,
                Value = DefaultValue
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
            volumeSlider.Current.ValueChanged += value => Current.Value = value.NewValue;
            volumeSlider.Current = sliderVolumeValue;

        }

        public void ChangeSliderValue(double value, char mathAction)
        {
            if (!(value > sliderVolumeValue.MinValue) && !(value < sliderVolumeValue.MaxValue)) return;
            switch(mathAction)
            {
                case '+':
                    volumeSlider.Current.Value += value;
                    break;
                case '-':
                    volumeSlider.Current.Value -= value;
                    break;
            }
        }



    }
}
