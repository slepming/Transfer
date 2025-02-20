using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.Graphics.UI.Containers;

/// <summary>
/// This class is responsible for binding keys when creating a playlist, handling playlist exceptions
/// </summary>
public partial class PlaylistContainer : Container, IKeyBindingHandler<GlobalAction>
{
    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        if (e.Repeat)
            return false;

        return true;
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }
}
