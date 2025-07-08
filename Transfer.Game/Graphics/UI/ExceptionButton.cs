using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI;

public partial class ExceptionButton : ClickableContainer
{
    public Box Background;

    private Colour4 backgroundColour;
    public Colour4 BackgroundColour {
        get => backgroundColour;
        set{
            if(backgroundColour == value) return;
            backgroundColour = value;
        }
    }

    public SpriteText SpriteText;

    private string text;
    public string Text {
        get => text;
        set{
            if(value == text) return;
            text = value;
        }
    }
    private Colour4 textColour;
    public Colour4 TextColour {
        get => textColour;
        set{
            if(value == textColour) return;
            textColour = value;
        }
    }

    public ExceptionButton()
    {
        Masking = true;
        BorderColour = new Colour4(255,255,255,0.5f);
        BorderThickness = 3;
        CornerRadius = 5;
        Colour = Colour4.White;
        InternalChildren = [
            Background = new Box{
                Colour = backgroundColour,
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.1f
            },
            SpriteText = new SpriteText{
                Text = Text,
                Colour = TextColour,
                Font = new FontUsage(TransferFonts.FiraCodeNerdFont, size: 20, fixedWidth: true),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            },
        ];
    }

    protected override void LoadComplete()
    {
        SpriteText.Text = Text;
        SpriteText.Colour = TextColour;
        base.LoadComplete();
    }


    protected override bool OnClick(ClickEvent e)
    {
        SpriteText.Text = "Good luck!";
        return base.OnClick(e);
    }

    protected override bool OnHover(HoverEvent e)
    {
        this.TransformTo(nameof(BorderColour),
            ColourInfo.GradientHorizontal(new Color4(255,255,255,0.4f),
            new Color4(255,255,255, 1f)), 700, Easing.InOutCubic);
        return base.OnHover(e);
    }
    protected override void OnHoverLost(HoverLostEvent e)
    {
        this.TransformTo(nameof(BorderColour), ColourInfo.GradientHorizontal(new Color4(255,255,255,1f),
            new Color4(255,255,255, 0.7f)), 700, Easing.InOutCubic);
        base.OnHoverLost(e);
    }
}
