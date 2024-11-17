using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
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
                    Size = new Vector2(200, 20),
                    TransferValueOnCommit = true,
                    KeyboardStep = 0f,
                    SelectionColour = Color4.DeepPink,
                },
            };
            sliderBar.Current = playbacklValue;
        }

        public void SetSliderBarValue(double value)
        {
            sliderBar.Current.Value = value;
        }

        public void SetMaxSliderValue(double value)
        {
            playbacklValue.MaxValue = value;
        }

    }
}
