using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferDirectorySelectorBreadcrumbDisplay : DirectorySelectorBreadcrumbDisplay
{
    public TransferDirectorySelectorBreadcrumbDisplay()
    {
        Masking = true;
        CornerRadius = 3;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(new Box()
        {
            Colour = Colour4.FromHex("#BBBBBB").Opacity(0.3f),
            RelativeSizeAxes = Axes.Both,
            Depth = 1
        });
    }

    protected override DirectorySelectorDirectory CreateDirectoryItem(DirectoryInfo directory, string displayName = null) => new BreadcrumbDisplayDirectory(directory, displayName);

    protected override DirectorySelectorDirectory CreateRootDirectoryItem() => new BreadcrumbDisplayComputer();

    protected partial class BreadcrumbDisplayComputer() : BreadcrumbDisplayDirectory(null, "Root")
    {
        protected override IconUsage? Icon => null;

        protected override Box Background => new Box()
        {
            Colour = Colour4.FromHex("#F2F2F2").Opacity(0.2f),
            Depth = 1,
            RelativeSizeAxes = Axes.Both
        };
    }

    protected partial class BreadcrumbDisplayDirectory : TransferDirectorySelectorDirectory
    {
        protected virtual Box Background => new Box()
        {
            Colour = Colour4.FromHex("#555555").Opacity(0.8f),
            Depth = 1,
            RelativeSizeAxes = Axes.Both,
        };

        public BreadcrumbDisplayDirectory(DirectoryInfo directory, string displayName = null)
            : base(directory, displayName)
        {
            Masking = true;
            CornerRadius = 3;
            BorderThickness = 2;
            BorderColour = Colour4.Transparent;
        }

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
            AddInternal(Background);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.TransformTo(nameof(BorderColour), (ColourInfo)Colour4.FromHex("#777777"), 300, Easing.Out);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.TransformTo(nameof(BorderColour), (ColourInfo)Colour4.Transparent, 300, Easing.Out);
        }
    }
}
