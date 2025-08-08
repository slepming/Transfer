using System.IO;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using Transfer.Game.Graphics.UIv2.Buttons;

namespace Transfer.Game.Graphics.UIv2;

public partial class PlaylistFileSelector(string initialPath = null) : TransferFileSelector(initialPath, TransferGameBase.VIDEO_EXTENSIONS)
{
    protected override Drawable CreateHiddenToggleButton() => null;

    protected override DirectorySelectorBreadcrumbDisplay CreateBreadcrumb() => null;

    protected override DirectorySelectorDirectory CreateParentDirectoryItem(DirectoryInfo directory) => new PlaylistDirectorySelectorParentDirectory(directory);

    public void UpdateFiles()
    {
        DirectoryInfo current = CurrentPath.Value;
        CurrentPath.Value = current.Parent;
        CurrentPath.Value = current;
    }
}
