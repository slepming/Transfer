using System.IO;
using osu.Framework.Graphics.Sprites;

namespace Transfer.Game.Graphics.UIv2.Buttons;

public partial class TransferDirectorySelectorParentDirectory(DirectoryInfo directory) : TransferDirectorySelectorDirectory(directory, "../")
{
    protected override IconUsage? Icon => FontAwesome.Solid.Folder;

    protected sealed override void ApplyHiddenState()
    {
    }
}
