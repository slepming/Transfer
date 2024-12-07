using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

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
            spriteText = new SpriteText{
                Colour = Colour4.White,
                Font = new FontUsage("FiraCodeNerdFont", size: 15, fixedWidth: true),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            }
        ];
    }


    protected override bool OnHover(HoverEvent e)
    {
        spriteText.FadeColour(Colour4.NavajoWhite, 400, Easing.InOutQuart);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        spriteText.FadeColour(Colour4.White, 300, Easing.InOutQuart);
        base.OnHoverLost(e);
    }





    [BackgroundDependencyLoader]
    private void load(){
        spriteText.Text = text ?? "Not loaded text";
    }
}
