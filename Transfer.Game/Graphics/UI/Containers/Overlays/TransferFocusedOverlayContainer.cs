using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers.Overlays;

public abstract partial class TransferFocusedOverlayContainer : FocusedOverlayContainer
{
    public bool Visible
    {
        get
        {
            if (State.Value == Visibility.Hidden) return false;
            else return true;
        }
    }

    protected abstract override void PopIn();

    protected abstract override void PopOut();
}
