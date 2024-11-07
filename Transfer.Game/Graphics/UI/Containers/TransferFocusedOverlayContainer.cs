using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers
{
    public abstract partial class TransferFocusedOverlayContainer : FocusedOverlayContainer
    {
        public bool Visible = true;
        public TransferFocusedOverlayContainer()
        {

        }
        public override void Hide()
        {
            Visible = false;
            base.Hide();
        }

        public override void Show()
        {
            Visible = true;
            base.Show();
        }
        protected abstract override void PopIn();

        protected abstract override void PopOut();

    }
}
