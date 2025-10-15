using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osuTK;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

/// <summary>
/// This class creates app window
/// </summary>
public abstract partial class TransferDialog : TransferFocusedOverlayContainer
{
    public Action<Exception> OnHandledException;

    private Box background;

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
        MaskingSmoothness = 3;
        CornerRadius = 5;
        CornerExponent = 6;
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
                Colour = Colour4.FromHex("#2d2d2d").Opacity(0.8f),
                RelativeSizeAxes = Axes.Both
            },
            PrefixBox ??= new()
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

    protected override void PopIn()
    {
        this.FadeIn(120, Easing.InOutQuad);
    }

    protected override void PopOut()
    {
        this.FadeOut(160, Easing.OutCirc);
    }
}
