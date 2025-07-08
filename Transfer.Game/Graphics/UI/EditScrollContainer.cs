using osu.Framework.Graphics;
using Transfer.Game.Graphics.UI.Containers;

namespace Transfer.Game.Graphics.UI
{
    public partial class EditScrollContainer : TransferScrollContainer<Drawable>
    {
        public EditScrollContainer(Direction direction) : base(direction)
        {
        }
    }
}
