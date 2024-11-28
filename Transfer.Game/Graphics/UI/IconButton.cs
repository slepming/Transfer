using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace Transfer.Game.Graphics.UI;

public partial class IconButton : ClickableContainer
{
    public Texture Texture { get; set; }
    private Sprite iconSprite;
    public IconButton()
    {
        Masking = true;
        InternalChildren = new Drawable[]
        {
            new Box{
                Colour = Colour4.Transparent,
                RelativeSizeAxes = Axes.Both
            },
            iconSprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both
            }

        };
    }
    protected override void LoadComplete()
    {
        iconSprite.Texture = Texture;
        base.LoadComplete();
    }
}
