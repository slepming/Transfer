using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Vulkan.Xlib;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

/// <summary>
/// This class creates a window according to the developer's requirements.
/// </summary>
public abstract partial class TransferDialog : TransferFocusedOverlayContainer
{
    private Box background;

    private TransparentButton closeButton;

    /// <summary>
    /// Content is used to add the insides of the window, while keeping the parts created by MenuWindow intact.
    /// </summary>
    protected TooltipContainer WindowContent = new TooltipContainer()
    {
        RelativeSizeAxes = Axes.Both,
        Size = new Vector2(1 / 1.1f, 1 / 1.2f),
        Anchor = Anchor.Centre,
        Origin = Anchor.Centre,
        Padding = new MarginPadding() { Top = 40 },
    };

    protected HeaderContainer PrefixBox;

    protected LocalisableString HeaderName;

    protected TransferDialog()
    {
        Depth = TransferGame.DIALOGS_DEPTH;
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

    public override void Hide()
    {
        foreach (var content in WindowContent.Children)
        {
            if (content is TextBox box)
            {
                box.Current.Value = string.Empty;
            }
        }

        base.Hide();
    }

    protected override void OnDrag(DragEvent e)
    {
        Position = e.ScreenSpaceMousePosition;
    }

    protected override void PopIn()
    {
        this.ScaleTo(1, 200, Easing.Out);
    }

    protected override void PopOut()
    {
        this.ScaleTo(0, 300, Easing.In);
    }
}
