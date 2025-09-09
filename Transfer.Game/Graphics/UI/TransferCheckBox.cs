using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.UI;

public partial class TransferCheckbox : Checkbox
{
    /// <summary>
    /// The color of the checkbox when the checkbox is checked. Defaults to White
    /// </summary>
    /// <remarks>
    /// The changes done to this property are only applied when <see cref="Checkbox.Current"/>'s value changes.
    /// </remarks>
    public Color4 CheckedColor { get; set; } = Colour4.FromHex("#BBBBBB").Opacity(.3f);

    /// <summary>
    /// The color of the checkbox when the checkbox is not checked. Default is a white with low opacity.
    /// </summary>
    /// <remarks>
    /// The changes done to this property are only applied when <see cref="Checkbox.Current"/>'s value changes.
    /// </remarks>
    public Color4 UncheckedColor { get; set; } = Colour4.FromHex("#BBBBBB").Opacity(.1f);

    /// <summary>
    /// The length of the duration between checked and unchecked.
    /// </summary>
    /// <remarks>
    /// Changes to this property only affect future transitions between checked and unchecked.
    /// Transitions between checked and unchecked that are already in progress are unaffected.
    /// </remarks>
    public int FadeDuration { get; set; } = 50;

    /// <summary>
    /// The text in the label.
    /// </summary>
    public LocalisableString LabelText
    {
        get => labelSpriteText.Text;
        set => labelSpriteText.Text = value;
    }

    /// <summary>
    /// The spacing between the checkbox and the label.
    /// </summary>
    public float LabelSpacing
    {
        get => fillFlowContainer.Spacing.X;
        set => fillFlowContainer.Spacing = new Vector2(value, 0);
    }

    public bool RightHandedCheckbox
    {
        get => fillFlowContainer.GetLayoutPosition(labelSpriteText) < -0.5f;
        set => fillFlowContainer.SetLayoutPosition(labelSpriteText, value ? -1 : 1);
    }

    private readonly SpriteText labelSpriteText;
    private readonly FillFlowContainer fillFlowContainer;

    public TransferCheckbox(Action<SpriteText> basic = null)
    {
        Box box;

        AutoSizeAxes = Axes.Both;
        Margin = new MarginPadding { Left = 10 };

        Child = fillFlowContainer = new FillFlowContainer
        {
            Direction = FillDirection.Horizontal,
            AutoSizeAxes = Axes.Both,
            Spacing = new Vector2(5, 0),
            Children =
            [
                new Container
                {
                    Masking = true,
                    CornerRadius = 3,
                    Size = new Vector2(30),
                    Child =
                        box = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                },
                labelSpriteText = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Depth = float.MinValue,
                    Font = FrameworkFont.Condensed
                }
            ]
        };
        basic?.Invoke(labelSpriteText);

        Current.BindValueChanged(e => box.FadeColour(e.NewValue ? CheckedColor : UncheckedColor, FadeDuration), true);
    }
}
