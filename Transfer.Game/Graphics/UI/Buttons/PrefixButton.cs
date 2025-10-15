using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osuTK;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI.Buttons;

/// <summary>
/// This button have <see href='Sprite'>prefix</see>
/// </summary>
public partial class PrefixButton : ClickableContainer
{
    private Sprite prefix;
    private SpriteText spriteText;

    public Texture Prefix = null;

    private LocalisableString text;

    public LocalisableString Text
    {
        get => text;
        set
        {
            if (text == value) return;
            text = value;
        }
    }

    public PrefixButton()
    {
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            new Box
            {
                Alpha = 0.2f,
                Colour = Colour4.White,
                RelativeSizeAxes = Axes.Both
            },
            new Container
            {
                // Masking = true,
                // BorderColour = Colour4.White.Opacity(90),
                // BorderThickness = 3,
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Size = new Vector2(1 / 12f, 1),
                Position = new Vector2(3, 0),
                Children =
                [
                    prefix = new Sprite()
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(1, 1 / 1.5f),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Texture = Prefix,
                    },
                ]
            },
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1 / 1.3f, 1),
                Masking = true,
                MaskingSmoothness = 5f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child =
                    spriteText = new SpriteText()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont, size: 12f),
                        Text = text
                    }
            }
        ];
    }
}
