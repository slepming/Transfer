using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK.Graphics;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI;

public partial class TransparentButton : ClickableContainer, IHasTooltip
{
    public string Text { get; set; }

    public new Color4 Colour { get; set; } = Color4.White;

    private SpriteText textObject;

    public LocalisableString TooltipText { get; }

    public TransparentButton(LocalisableString? tooltipText = null)
    {
        TooltipText = tooltipText ?? string.Empty;
        InternalChild = textObject = new SpriteText
        {
            Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont),
            Colour = this.Colour
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        textObject.Text = Text;
    }
}
