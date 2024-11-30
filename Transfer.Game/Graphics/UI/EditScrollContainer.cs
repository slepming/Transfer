using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
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
