using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferDirectorySelectorBreadcrumbDisplay : DirectorySelectorBreadcrumbDisplay
{
    protected override Drawable CreateCaption() => new SpriteText()
    {
        Text = "Current: ",
        Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont)
    };

    protected override DirectorySelectorDirectory CreateDirectoryItem(DirectoryInfo directory, string displayName = null) => new BreadcrumbDisplayDirectory(directory, displayName);

    protected override DirectorySelectorDirectory CreateRootDirectoryItem() => new BreadcrumbDisplayComputer();

    protected partial class BreadcrumbDisplayComputer() : BreadcrumbDisplayDirectory(null, "Computer")
    {
        protected override IconUsage? Icon => null;
    }

    protected partial class BreadcrumbDisplayDirectory(DirectoryInfo directory, string displayName = null) : TransferDirectorySelectorDirectory(directory, displayName)
    {
        protected sealed override void ApplyHiddenState()
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Flow.Add(new SpriteIcon()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Icon = FontAwesome.Solid.ChevronRight,
                Size = new Vector2(FONT_SIZE / 2)
            });
        }
    }
}
