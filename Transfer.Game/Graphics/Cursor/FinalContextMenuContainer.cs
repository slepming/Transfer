using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics.Cursor
{
    public partial class FinalContextMenuContainer : ContextMenuContainer
    {
        protected override Menu CreateMenu() => new FinalMenu(osu.Framework.Graphics.Direction.Vertical);

    }
}
