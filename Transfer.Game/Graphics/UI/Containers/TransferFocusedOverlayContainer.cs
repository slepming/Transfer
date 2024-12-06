using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers
{
    public abstract partial class TransferFocusedOverlayContainer : FocusedOverlayContainer
    {
        public bool Visible
        {
            get
            {
                if(State.Value == Visibility.Hidden) return false;
                else return true;
            }
        }
        public TransferFocusedOverlayContainer()
        {

        }
        protected abstract override void PopIn();

        protected abstract override void PopOut();

    }
}
