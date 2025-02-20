using System;
using System.IO;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Graphics;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI.Explorer;

public partial class ExplorerButton : ClickableContainer
{
    private Action<string, bool> action;

    public new Action<string, bool> Action
    {
        get => action;
        set
        {
            action = value;
            Enabled.Value = action != null;
        }
    }

    /// <summary>
    /// Current explorer position in path
    /// </summary>
    public string Current = string.Empty;

    /// <summary>
    /// Last explorer position in path
    /// </summary>
    public string Last;

    public bool IsDirectory { get; set; }

    private Box background;
    private SpriteText textSprite;

    public LocalisableString Text
    {
        get => textSprite.Text;
        set => textSprite.Text = value;
    }

    public Color4 BackgroundColour
    {
        get => background.Colour;
        set => background.Colour = value;
    }

    public float BackgroundAlpha
    {
        get => background.Alpha;
        set => background.Alpha = value;
    }

    public Color4 ForegroundColour
    {
        get => textSprite.Colour;
        set => textSprite.Colour = value;
    }

    public ExplorerButton()
    {
        Last = Path.GetDirectoryName(Current);
        InternalChildren =
        [
            background = new Box
            {
                Colour = Colour4.White,
                Alpha = 0.5f,
                RelativeSizeAxes = Axes.Both
            },
            textSprite = new SpriteText()
            {
                Text = "",
                Colour = Colour4.White,
                Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont)
            }
        ];
    }

    protected override bool OnClick(ClickEvent e)
    {
        if (Enabled.Value)
            Action?.Invoke(Current, IsDirectory);
        return true;
    }
}
