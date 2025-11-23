using System.Numerics;
using osu.Framework.Bindables;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics.UIv2;

public partial class VideoPlaybackSlider : VideoPlaybackSlider<double>
{
    public BindableDouble UserCommit { get; } = new();

    protected override void OnUserChange(double value)
    {
        UserCommit.Value = value;
    }
}

public partial class VideoPlaybackSlider<T> : TransferBasicSliderBar<T>
    where T : struct, INumber<T>, IMinMaxValue<T>;
