using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace Transfer.Game.Graphics.UI.Containers;

public abstract partial class TransferFocusedOverlayContainer : FocusedOverlayContainer
{
    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Repeat)
            return false;

        switch (e.Key)
        {
            case Key.Escape:
                Hide();
                return true;
        }

        return base.OnKeyDown(e);
    }

    protected abstract override void PopIn();

    protected abstract override void PopOut();
}
