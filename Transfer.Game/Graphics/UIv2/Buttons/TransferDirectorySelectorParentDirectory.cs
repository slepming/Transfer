using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace Transfer.Game.Graphics.UIv2.Buttons;

internal partial class TransferDirectorySelectorParentDirectory(DirectoryInfo directory) : TransferDirectorySelectorDirectory(directory, "../")
{
    [Resolved]
    private Bindable<DirectoryInfo> currentDirectory { get; set; }

    protected override IconUsage? Icon => FontAwesome.Solid.Folder;

    protected sealed override void ApplyHiddenState()
    {
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Repeat)
            return false;

        if (e.Key == Key.BackSpace)
            currentDirectory.Value = Directory;
        return base.OnKeyDown(e);
    }
}
