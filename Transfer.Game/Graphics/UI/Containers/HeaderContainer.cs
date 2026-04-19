using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Buttons;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class HeaderContainer : TransferContainer
{
    private LocalisableString headerText;
    private StringButton closeButton;

    public LocalisableString HeaderText
    {
        get => headerText;
        set
        {
            if (headerText.Equals(value)) return;

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
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.White.Opacity(0.05f),
            },
            new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = new FontUsage(size: 25, family: TransferFonts.FiraCodeNerdFontLight),
                Text = headerText
            },
            closeButton = new StringButton
            {
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
                Text = "Close",
                BackgroundColour = Colour4.Transparent
            },
        ];
    }
}
