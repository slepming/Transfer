using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace Transfer.Game.Graphics.UI;

public partial class BoxButton : ClickableContainer
{
    private Box background;
    private SpriteText spriteText;

    private string text;

    public string Text {
        get => text;
        set{
            if(text == value) return;
            text = value;
        }
    }

    public BoxButton()
    {
        Masking = true;
        BorderColour = FrameworkColour.BlueDark;
        Colour = Colour4.White;
        BorderThickness = 2;
        CornerRadius = 4;
        InternalChildren = [
            background = new Box{
                Alpha = 0.1f,
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Chocolate,
            },
            spriteText = new SpriteText{
                Colour = Colour4.White,
                Font = new FontUsage("FiraCodeNerdFont", size: 20, fixedWidth: true),
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            }
        ];
    }





    [BackgroundDependencyLoader]
    private void load(){
        spriteText.Text = text ?? "Not loaded text";
    }
}
