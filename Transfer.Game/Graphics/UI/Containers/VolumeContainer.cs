using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;

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

        public float SliderWidth { get; }


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
                    Width = 200,
                    Height = 20,
                    TransferValueOnCommit = true,
                    KeyboardStep = 0.1f,
                    SelectionColour = Color4.DeepPink,
                },
            };
            volumeSlider.Current.ValueChanged += value => Current.Value = value.NewValue;
            volumeSlider.Current = sliderVolumeValue;
            SliderWidth = volumeSlider.Width;

        }

        public void SetSliderValue(double value)
        {
            volumeSlider.Current.Value = value;
        }

        public void ChangeSliderValue(double value, char mathAction)
        {
            if (value < sliderVolumeValue.MinValue && value > sliderVolumeValue.MaxValue) return;
            switch (mathAction)
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
