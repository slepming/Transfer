using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UI.Containers;

namespace Transfer.Game.Graphics.UIv2.Containers;

public partial class VolumeContainer : TransferContainer
{
    public VolumeSlider Slider { get; private set; }

    public VolumeContainer()
    {
        AutoSizeAxes = Axes.X;
        Height = 30;
        Masking = true;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Slider = new()
        {
        };
    }
}
