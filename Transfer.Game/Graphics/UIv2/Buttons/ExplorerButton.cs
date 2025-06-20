using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Logging;

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
            new SpriteText()
            {
                Text = getTreeText(Text),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both
            },
        ];
    }

    private string getTreeText(LocalisableString text)
    {
        if (string.IsNullOrEmpty(Path.GetFileName(text.ToString()))) return "Undefined";

        string normalizedPath = Path.GetFullPath(text.ToString());

        string[] parts = normalizedPath.Split(Path.DirectorySeparatorChar);

        return new string(' ', parts.Length - 2) + Path.GetFileName(normalizedPath);
    }

    protected override bool OnClick(ClickEvent e)
    {
        Action?.Invoke(Text.ToString());
        return base.OnClick(e);
    }
}
