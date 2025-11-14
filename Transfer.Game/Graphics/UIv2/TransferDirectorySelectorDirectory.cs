using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using Transfer.Game.Extensions;
using Box = osu.Framework.Graphics.Shapes.Box;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferDirectorySelectorDirectory : DirectorySelectorDirectory
{
    protected override IconUsage? Icon => Directory.Name.Contains(Path.DirectorySeparatorChar) ? FontAwesome.Solid.Database : FontAwesome.Regular.Folder;
    protected Box Background { get; private set; }

    public TransferDirectorySelectorDirectory(DirectoryInfo directory, string displayName = null)
        : base(directory, displayName)
    {
        Masking = true;
        CornerRadius = 5f;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(Background = CreateBackground().With(b => b.RelativeSizeAxes = Axes.Both));
    }

    protected virtual Box CreateBackground() => new()
    {
        Colour = Colour4.Transparent
    };

    protected override SpriteText CreateSpriteText() => new()
    {
        Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont)
    };

    protected override bool OnHover(HoverEvent e)
    {
        Background.FadeColour(Colour4.White.Opacity(.2f), 200, Easing.OutQuint);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        Background.FadeColour(Colour4.White.Opacity(0), 200, Easing.OutQuint);
        base.OnHoverLost(e);
    }
}
