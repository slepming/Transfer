using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using Transfer.Game.Graphics.UIv2.Sliders;

namespace Transfer.Game.Graphics.UI.Containers;

/// <summary>
/// It is just slider as playback line(without input action)
/// </summary>
public partial class VideoPlaybackContainer : Container
{
    public VideoPlaybackSlider SliderBar { get; private set; }

    /// <summary>
    /// Max value for slider
    /// </summary>
    public double Duration = 0;

    /// <summary>
    /// Min value for slide
    /// </summary>
    public double MinValue = 0;

    private BindableDouble playback;

    [BackgroundDependencyLoader]
    private void load()
    {
        playback = new BindableDouble()
        {
            MinValue = MinValue,
            MaxValue = Duration,
            Default = 0
        };

        InternalChildren =
        [
            SliderBar = new()
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
        SliderBar.Current = playback;
    }

    public void SetSliderBarValue(double value)
    {
        SliderBar.Current.Value = value;
    }
}
