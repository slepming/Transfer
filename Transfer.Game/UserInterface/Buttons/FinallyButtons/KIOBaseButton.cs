using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KIO.Game.UserInterface.Buttons.FinallyButtons
{
    public partial class KIOBaseButton : TransferButton
    {
        public KIOBaseButton(string text, Action action) : base(text, action)
        {
            Size = new osuTK.Vector2(100,30);
        }
        public KIOBaseButton(string text) : base(text)
        {
            Size = new osuTK.Vector2(100,30);
        }

    }
}