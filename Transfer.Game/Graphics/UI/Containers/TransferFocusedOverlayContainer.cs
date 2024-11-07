using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers
{
    public partial class TransferFocusedOverlayContainer : FocusedOverlayContainer
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
        protected override void PopIn()
        {
            throw new NotImplementedException();
        }

        protected override void PopOut()
        {
            throw new NotImplementedException();
        }

    }
}
