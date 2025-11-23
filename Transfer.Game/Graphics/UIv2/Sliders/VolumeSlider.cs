using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osuTK.Graphics;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics.UIv2.Sliders;

public partial class VolumeSlider : VolumeSlider<double>
{
    [Resolved]
    private FrameworkConfigManager config { get; set; }

    private readonly BindableDouble current = new()
    {
        MinValue = 0,
        MaxValue = 1,
        Value = 0.5,
    };

    [BackgroundDependencyLoader]
    private void load()
    {
        current.Value = config.Get<double>(FrameworkSetting.VolumeMusic);
    }
}

public partial class VolumeSlider<T> : TransferBasicSliderBar<T> where T : struct, INumber<T>, IMinMaxValue<T>
{
    public VolumeSlider()
    {
        BackgroundColour = Color4.Transparent;
        SelectionColour = Colour4.White;
        SelectionCircleColour = Colour4.Transparent;
        FocusColour = Colour4.White;
        FocusBorderThickness = 3;
    }
}
