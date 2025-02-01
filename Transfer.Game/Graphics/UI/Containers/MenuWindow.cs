using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;
using Transfer.Game.Graphics.UI.Containers.Overlays;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class MenuWindow : TransferFocusedOverlayContainer
{
    private Box background;

    private TransparentButton closeButton, preferencesMenu;

    public MenuWindow()
    {
        RelativeSizeAxes = Axes.Both;
        Size = new Vector2(1 / 1.5f, 1 / 1.2f);
        Masking = true;
        BorderColour = Colour4.White;
        BorderThickness = 3;
        InternalChildren =
        [
            new Box
            {
                Alpha = 0.3f,
                Colour = Colour4.Gray,
                RelativeSizeAxes = Axes.Both
            },
        ];
    }

    protected override void PopIn()
    {
        this.ScaleTo(1, 400, Easing.Out);
    }

    protected override void PopOut()
    {
        this.ScaleTo(0, 200, Easing.OutExpo);
    }
}
