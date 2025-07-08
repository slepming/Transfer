using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class TransferContainer : Container
{
    public bool ShowBorder
    {
        set
        {
            Masking = value;

            if (value)
            {
                BorderColour = Colour4.White;
                BorderThickness = 2;
            }
            else
            {
                BorderColour = default;
                BorderThickness = 0;
            }
        }
    }
}
