using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferDirectorySelectorBreadcrumbDisplay : DirectorySelectorBreadcrumbDisplay
{
    public TransferDirectorySelectorBreadcrumbDisplay()
    {
        Masking = true;
        CornerRadius = 5;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddRangeInternal([
            new Box()
            {
                Colour = Colour4.FromHex("#BBBBBB").Opacity(0.3f),
                RelativeSizeAxes = Axes.Both,
                Depth = 2
            },
        ]);
    }

    protected override DirectorySelectorDirectory CreateDirectoryItem(DirectoryInfo directory, string displayName = null) => new BreadcrumbDisplayDirectory(directory, displayName);

    protected override DirectorySelectorDirectory CreateRootDirectoryItem() => new BreadcrumbDisplayComputer();

    protected partial class BreadcrumbDisplayComputer() : BreadcrumbDisplayDirectory(null, "System")
    {
        protected override IconUsage? Icon => null;

        protected override Box CreateBackground() => new Box
        {
            Colour = Colour4.FromHex("#F2F2F2").Opacity(.2f),
            Depth = 1,
        };
    }

    protected partial class BreadcrumbDisplayDirectory : TransferDirectorySelectorDirectory
    {
        public BreadcrumbDisplayDirectory(DirectoryInfo directory, string displayName = null)
            : base(directory, displayName)
        {
            Masking = true;
            CornerRadius = 3;
            BorderThickness = 2;
            BorderColour = Colour4.Transparent;
        }

        protected override Box CreateBackground() => new Box()
        {
            Colour = Colour4.FromHex("#555555").Opacity(0.8f),
            Depth = 1,
            RelativeSizeAxes = Axes.Both,
        };

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
