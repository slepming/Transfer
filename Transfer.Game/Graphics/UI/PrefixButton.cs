using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI;

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
            prefix = new Sprite()
            {
                Width = 15,
                Height = 15,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Texture = Prefix,
            },
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1 / 1.2f, 1),
                Masking = true,
                MaskingSmoothness = 5f,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child =
                    spriteText = new SpriteText()
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont),
                        Text = text
                    }
            }
        ];
    }
}
