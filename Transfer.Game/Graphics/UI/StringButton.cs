using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Graphics;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI;

public partial class StringButton : ClickableContainer, IHasTooltip
{
    private Box background;
    private SpriteText spriteText;

    private LocalisableString text;

    public LocalisableString Text
    {
        get => text;
        set
        {
            if(text == value) return;
            text = value;
        }
    }

    public Color4 BackgroundColour
    {
        get => background.Colour;
        set => background.Colour = value;
    }

    public LocalisableString TooltipText { get; }

    public StringButton(LocalisableString tooltipText = new LocalisableString())
    {
        TooltipText = tooltipText;
        Masking = true;
        Colour = Colour4.White;
        InternalChildren =
        [
            background = new Box()
            {
                Alpha = 0.3f,
                Colour = Colour4.White,
                RelativeSizeAxes = Axes.Both
            },
            spriteText = new SpriteText
            {
                Colour = Colour4.White,
                Font = new FontUsage(TransferFonts.FiraCodeNerdFont, size: 15),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            }
        ];
    }

    protected override bool OnHover(HoverEvent e)
    {
        spriteText.FadeColour(Colour4.NavajoWhite, 400, Easing.Out);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        spriteText.FadeColour(Colour4.White, 300, Easing.In);
        base.OnHoverLost(e);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        spriteText.Text = text.ToString() ?? "Not loaded text";
    }

}
