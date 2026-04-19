using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI.Buttons;

public partial class PlaylistButton : ClickableContainer
{
    private string current = string.Empty;

    /// <summary>
    /// Text for button
    /// </summary>
    public string Current
    {
        get => current;
        set
        {
            if (current == value) return;
            current = value;
        }
    }

    /// <summary>
    /// Invoke on user click and give path to file
    /// </summary>
    public new Action<string> Action;

    private Box background;
    private Container content;
    private readonly SpriteText text;

    public PlaylistButton()
    {
        Masking = true;
        CornerRadius = 5;
        BorderThickness = 2;
        BorderColour = Colour4.White;
        InternalChildren =
        [
            background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black.Opacity(0.3f)
            },
            content = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(10),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children =
                [
                    text = new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Font = new FontUsage(size: 16, family: TransferFonts.FiraCodeNerdFontLight),
                    }
                ]
            }
        ];
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        text.Text = current;
    }

    protected override bool OnClick(ClickEvent e)
    {
        Action?.Invoke(current);
        return base.OnClick(e);
    }

    public override void Show()
    {
        this.FadeIn(300, Easing.OutQuint);
        base.Show();
    }

    public override void Hide()
    {
        this.FadeOut(300, Easing.OutQuint);
        base.Hide();
    }
}
