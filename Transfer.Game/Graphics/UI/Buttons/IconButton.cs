using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK;

namespace Transfer.Game.Graphics.UI.Buttons;

public abstract partial class IconButton(Action<SpriteText> defaultCreationParameters) : ClickableContainer
{
    protected const float FONT_SIZE = 24;
    protected abstract IconUsage? Icon { get; }

    protected FillFlowContainer Flow;

    public LocalisableString Text
    {
        get => text;
        set
        {
            if (text.Equals(value)) return;

            text = value;
        }
    }

    private LocalisableString text;

    protected virtual SpriteText CreateSpriteText()
    {
        var spriteText = new SpriteText();
        defaultCreationParameters?.Invoke(spriteText);
        return spriteText;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AutoSizeAxes = Axes.Both;

        InternalChild = Flow = new FillFlowContainer()
        {
            AutoSizeAxes = Axes.Both,
            Margin = new MarginPadding { Vertical = 2, Horizontal = 5 },
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(5)
        };

        if (Icon != null)
        {
            Flow.Add(new SpriteIcon()
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Icon = Icon.Value,
                Size = new Vector2(FONT_SIZE)
            });
        }

        Flow.Add(CreateSpriteText().With(st =>
        {
            st.Anchor = Anchor.CentreLeft;
            st.Origin = Anchor.CentreLeft;
            st.Text = text;
        }));
    }
}
