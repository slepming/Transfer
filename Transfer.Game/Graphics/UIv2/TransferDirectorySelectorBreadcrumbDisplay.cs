using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
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

        protected override Box CreateBackground() => new()
        {
            Colour = Colour4.FromHSL(0, 0, 0.4f),
            Depth = 1,
        };
    }

    protected partial class BreadcrumbDisplayDirectory : TransferDirectorySelectorDirectory
    {
        public BreadcrumbDisplayDirectory(DirectoryInfo directory, string displayName = null)
            : base(directory, displayName)
        {
            Masking = true;
            CornerRadius = 5;
            BorderColour = Colour4.FromHSL(0, 0, 0.4f);
            BorderThickness = 2;
        }

        protected override Box CreateBackground() => new()
        {
            Colour = Colour4.FromHSL(0, 0, 0.3f, 0.8f),
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

        protected override bool OnHover(HoverEvent e)
        {
            this.RotateTo(3, 150, Easing.OutQuad);
            return Handle(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.RotateTo(0, 50, Easing.OutQuad);
        }
    }
}
