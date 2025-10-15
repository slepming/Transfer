using System.Numerics;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics.UIv2;

public partial class VideoPlaybackSlider<T> : TransferBasicSliderBar<T>
    where T : struct, INumber<T>, IMinMaxValue<T>
{
}
