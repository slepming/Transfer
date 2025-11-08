using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.UI.Containers;

/// <summary>
/// It is just slider as playback line(without input action)
/// </summary>
public partial class VideoPlaybackContainer : Container
{
    private TransferBasicSliderBar<double> sliderBar;

    public double Duration = 0;
    public double MinValue = 0;
    public BindableDouble Playback { get; private set; }

    [BackgroundDependencyLoader]
    private void load()
    {
        Playback = new BindableDouble()
        {
            MinValue = MinValue,
            MaxValue = Duration,
            Default = 0
        };

        InternalChildren =
        [
            sliderBar = new TransferBasicSliderBar<double>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                TransferValueOnCommit = true,
                KeyboardStep = 0f,
                SelectionColour = Color4.White,
                SelectionCircleColour = Color4.Transparent,
                BackgroundColour = Color4.Gray.Opacity(50),
                FocusBorderThickness = 0f,
            },
        ];
        sliderBar.Current = Playback;
    }

    public void SetSliderBarValue(double value)
    {
        sliderBar.Current.Value = value;
    }
}
