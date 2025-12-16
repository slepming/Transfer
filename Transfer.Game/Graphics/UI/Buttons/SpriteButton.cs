using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;

namespace Transfer.Game.Graphics.UI.Buttons;

public partial class SpriteButton : ClickableContainer
{
    public string TextureName { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        Texture texture = textureStore?.Get(TextureName);
        if (texture is null) Logger.Log($"Texture with name {TextureName} is null");
        InternalChildren =
        [
            new Box
            {
                Depth = 2,
                RelativeSizeAxes = Axes.Both,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Colour = Colour4.White,
                Alpha = 0,
            },
            new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = textureStore?.Get(TextureName),
                Depth = 1
            },
            new SpriteText
            {
                Text = texture is null ? translateWordToSymbol(TextureName) : null,
                Colour = Colour4.White,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            }
        ];
    }

    private string translateWordToSymbol(string word)
    {
        Dictionary<string, string> wordToSymbol = new Dictionary<string, string>
        {
            { "back", "<<" },
            { "next", ">>" },
            { "nextSeek", ">" },
            { "backSeek", "<" },
            { "pause", "\u23f8" },
            { "play", "\u25b6" }
        };

        if (wordToSymbol.TryGetValue(word, out string symbol))
            return symbol;

        return word;
    }
}
