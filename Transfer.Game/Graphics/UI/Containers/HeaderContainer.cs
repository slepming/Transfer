using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class HeaderContainer : Container
{
    private LocalisableString headerText;
    private TransparentButton closeButton;

    public LocalisableString HeaderText
    {
        get => headerText;
        set
        {
            if (headerText == value) return;
            headerText = value;
        }
    }

    public HeaderContainer()
    {
        Anchor = Anchor.TopCentre;
        Origin = Anchor.TopCentre;
        RelativeSizeAxes = Axes.X;
        Height = 40;
        Masking = true;
        BorderColour = Colour4.White;
        BorderThickness = 2;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray.Opacity(0.3f),
            },
            new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = new FontUsage(size: 25, family: TransferFonts.FiraCodeNerdFontLight),
                Text = headerText
            },
            closeButton = new TransparentButton() // Idk, this button can't change position with Origin
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.Centre,
                Text = "Close",
            },
        ];
    }
}
