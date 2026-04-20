using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osu.Framework.Input.Events;

namespace Transfer.Game.Graphics.UI.Buttons;

public partial class DynamicToolButton : ClickableContainer
{
    public List<IconUsage> Icons = new List<IconUsage>();
    public int Cursor { get; private set; } = 0;

    private Box background;
    private SpriteIcon icon;

    [BackgroundDependencyLoader]
    private void load()
    {
        icon = new SpriteIcon()
        {
            Icon = Icons[Cursor],
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.6f, 0.6f),
        };

        background = new Box
        {
            RelativeSizeAxes = Axes.Both,
        };

        AddRange([icon]);
    }

    protected override bool OnMouseDown(MouseDownEvent e)
    {
        ++Cursor;
        if (Cursor >= Icons.Count)
            Cursor = 0;
        icon.ScaleTo(0.5f, 600, Easing.OutCubic);
        return base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseUpEvent e)
    {
        icon.Icon = Icons[Cursor];
        icon.ScaleTo(0.8f, 400, Easing.OutElastic);
        base.OnMouseUp(e);
    }

    public void SetCursor(int cur)
    {
        Cursor = cur;
        icon.Icon = Icons[Cursor];
    }
}
