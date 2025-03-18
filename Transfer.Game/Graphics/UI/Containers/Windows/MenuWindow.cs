using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Containers.Overlays;

namespace Transfer.Game.Graphics.UI.Containers.Windows;

/// <summary>
/// This class creates a window according to the developer's requirements.
/// </summary>
public abstract partial class MenuWindow : TransferFocusedOverlayContainer
{
    private Box background;

    private TransparentButton closeButton;

    /// <summary>
    /// Content is used to add the insides of the window, while keeping the parts created by MenuWindow intact.
    /// </summary>
    protected Container WindowContent = new Container
    {
        RelativeSizeAxes = Axes.Both,
        Size = new Vector2(1 / 1.1f, 1 / 1.2f),
        Anchor = Anchor.Centre,
        Origin = Anchor.Centre
    };

    protected HeaderContainer PrefixBox;

    protected LocalisableString HeaderName;

    protected MenuWindow()
    {
        RelativeSizeAxes = Axes.Both;
        Size = new Vector2(1 / 1.2f, 1 / 1.5f);
        Masking = true;
        MaskingSmoothness = 5;
        BorderColour = Colour4.White;
        BorderThickness = 3;
        Anchor = Anchor.Centre;
        Origin = Anchor.Centre;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        InternalChildren =
        [
            background = new Box
            {
                Alpha = 0.3f,
                Colour = Colour4.Gray,
                RelativeSizeAxes = Axes.Both
            },
            PrefixBox = new HeaderContainer()
            {
                HeaderText = HeaderName,
            }
        ];
        AddInternal(WindowContent);
    }

    protected override void PopIn()
    {
        this.ScaleTo(1, 600, Easing.Out);
    }

    protected override void PopOut()
    {
        this.ScaleTo(0, 200, Easing.OutExpo);
    }
}
