using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UIv2.Buttons;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferFileSelector(string initialPath = null, string[] validFileExtensions = null) : FileSelector(initialPath, validFileExtensions)
{
    protected override Drawable CreateHiddenToggleButton() => new TransferCheckbox(s => s.Font = new FontUsage(TransferFonts.Oswald))
    {
        LabelText = "Toggle hidden items",
    }.With(b => b.Current.BindValueChanged(_ => ShowHiddenItems.Toggle()));

    protected override ScrollContainer<Drawable> CreateScrollContainer() => new TransferScrollContainer();

    protected override DirectorySelectorBreadcrumbDisplay CreateBreadcrumb() => new TransferDirectorySelectorBreadcrumbDisplay();

    protected override DirectorySelectorDirectory CreateDirectoryItem(DirectoryInfo directory, string displayName = null) => new TransferDirectorySelectorDirectory(directory, displayName);

    protected override DirectorySelectorDirectory CreateParentDirectoryItem(DirectoryInfo directory) => new TransferDirectorySelectorParentDirectory(directory);

    protected override DirectoryListingFile CreateFileItem(FileInfo file) => new TransferListingFile(file);

    private partial class TransferListingFile(FileInfo file) : DirectoryListingFile(file)
    {
        protected override IconUsage? Icon => FontAwesome.Regular.FileVideo;

        [BackgroundDependencyLoader]
        private void load()
        {
            Flow.AutoSizeAxes = Axes.X;
            Flow.Height = 16;

            AddInternal(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black.Opacity(.1f),
            });
        }

        protected override SpriteText CreateSpriteText() => new SpriteText()
        {
            Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont, size: FONT_SIZE)
        };
    }
}
