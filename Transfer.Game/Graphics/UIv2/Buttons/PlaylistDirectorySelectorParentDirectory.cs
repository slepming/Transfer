using System.IO;
using osu.Framework.Input.Events;

namespace Transfer.Game.Graphics.UIv2.Buttons;

internal partial class PlaylistDirectorySelectorParentDirectory(DirectoryInfo directory) : TransferDirectorySelectorParentDirectory(directory)
{
    protected override bool OnClick(ClickEvent e)
    {
        return false;
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        return false;
    }
}
