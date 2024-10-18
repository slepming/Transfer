using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerContainer : FocusedOverlayContainer
{
    protected override bool BlockScrollInput { get; }
    protected override bool StartHidden { get; }
    protected override bool BlockPositionalInput { get; }

    public ExplorerContainer(bool blockScrollInput = true, bool startHidden = true, bool blockPositionalInput = true)
    {
        Masking = true;
        BlockScrollInput = blockScrollInput;
        StartHidden = startHidden;
        BlockPositionalInput = blockPositionalInput;
        RelativeSizeAxes = Axes.Both;
        Origin = Anchor.Centre;
        Anchor = Anchor.Centre;
        Size = new Vector2(0.9f, 0.9f);
        InternalChildren = [
            new SpriteText{
                Text = "Javana",
                Font = new FontUsage(size:20),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Width = 200,
                Height = 200
            }
        ];
    }
    protected override void PopIn()
    {
        this.FadeTo(1, 600, Easing.In);
    }

    protected override void PopOut()
    {
        this.FadeTo(0.1f, 600, Easing.Out);
    }
}
