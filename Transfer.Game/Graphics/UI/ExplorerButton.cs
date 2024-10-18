using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace Transfer.Game.Graphics.UI;

public partial class ExplorerButton : ClickableContainer
{
    private string text;
    public string Text {
        get => text;
        set{
            if(value == text) return;
            text = value;
        }
    }

    private SpriteText spriteText;
    public ExplorerButton()
    {
        Masking = true;
        Width = 100;
        Height = 50;
        InternalChildren = [
            new Box{
                Alpha = 0.1f,
                Colour = Colour4.White,
            },
            new SpriteText{
                Colour = Colour4.White,
                Origin =  Anchor.Centre,
                Anchor = Anchor.Centre,

            }
        ];
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        spriteText.Text = text ?? "None";
    }
}
