using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI;

public partial class TransparentButton : ClickableContainer
{
    public string Text { get; set; }

    public Color4 Colour { get; set; } = Color4.White;

    private SpriteText textObject;

    public TransparentButton()
    {
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
