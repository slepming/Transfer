using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

namespace Transfer.Game.Graphics.UIv2.Buttons;

public partial class ExplorerButton : ClickableContainer, IHasText
{
    public LocalisableString Text { get; set; }

    public new Action<string> Action { get; set; }

    public ExplorerButton()
    {
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
        Masking = true;
        BorderColour = Colour4.White.Opacity(0.5f);
        BorderThickness = 2;
        CornerRadius = 2;
        Width = 200;
        Height = 50;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Children =
        [
            new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Black.Opacity(0.3f),
            },
            new TextFlowContainer()
            {
                Text = Path.GetFileNameWithoutExtension(Text.ToString()),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both
            },
        ];
    }

    protected override bool OnClick(ClickEvent e)
    {
        Action?.Invoke(Text.ToString());
        return base.OnClick(e);
    }
}
