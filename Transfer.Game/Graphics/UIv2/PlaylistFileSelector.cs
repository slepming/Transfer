using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Buttons;
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

    protected Drawable CreateUpdateButton() => new ToolsButton(FontAwesome.Regular.Circle, s =>
    {
        s.Font = new FontUsage(size: 13f, family: TransferFonts.FiraCodeNerdFont);
    })
    {
        Action = UpdateFiles,
        Text = "Update Files"
    };

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(CreateUpdateButton().With(b =>
        {
            b.Anchor = Anchor.TopRight;
            b.Origin = Anchor.TopRight;
        }));
    }
}
