using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osuTK.Graphics;
using Transfer.Game.UserInterface;

namespace Transfer.Game.Graphics.UI.Containers
{

    public partial class VideoPlaybackContainer : Container
    {
        private TransferBasicSliderBar<double> sliderBar;
        private BindableDouble playbacklValue;


        private double maxValue = 0;
        private double minValue = 0;
        public VideoPlaybackContainer()
        {
            playbacklValue = new BindableDouble()
            {
                MinValue = minValue,
                MaxValue = maxValue,
                Default = 0
            };

            InternalChildren = new Drawable[]
            {
                sliderBar = new TransferBasicSliderBar<double>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 200,
                    Height = 20,
                    TransferValueOnCommit = true,
                    KeyboardStep = 0f,
                    SelectionColour = Color4.HotPink,
                },
            };
            sliderBar.Current = playbacklValue;
        }

        public void SetSliderWidth(float value)
        {
            sliderBar.Width = value;
        }

        public float GetSliderWidth()
        {
            return sliderBar.Width;
        }

        public void SetSliderBarValue(double value)
        {
            sliderBar.Current.Value = value;
        }

        public void SetMaxSliderValue(double value)
        {
            playbacklValue.MaxValue = value;
            sliderBar.Current = playbacklValue;
        }

    }
}
