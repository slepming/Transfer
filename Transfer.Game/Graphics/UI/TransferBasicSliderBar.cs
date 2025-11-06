using System.Numerics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using Vector2 = osuTK.Vector2;

namespace Transfer.Game.Graphics.UI;

/// <summary>
/// Set basic for Transfer slider bar.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>I don't know how I can set Size for Relative Axes. Because of this set please width and height manually</remarks>
public partial class TransferBasicSliderBar<T> : SliderBar<T> where T : struct, INumber<T>, IMinMaxValue<T>
{
    public Color4 BackgroundColour
    {
        get => Box.Colour;
        set => Box.Colour = value;
    }

    public Color4 SelectionColour
    {
        get => SelectionBox.Colour;
        set => SelectionBox.Colour = value;
    }

    private Color4 focusColour = Color4.Transparent;

    public Color4 FocusColour
    {
        get => focusColour;
        set
        {
            focusColour = value;
            updateFocus();
        }
    }

    private float focusBorderThickness = 0f;

    public float FocusBorderThickness
    {
        get => focusBorderThickness;
        set
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (value == focusBorderThickness) return;

            focusBorderThickness = value;
        }
    }

    /// <summary>
    /// Color for circle in the end selection.
    /// </summary>
    public Color4 SelectionCircleColour
    {
        get => SelectionEndCircle.Colour;
        set => SelectionEndCircle.Colour = value;
    }

    protected readonly Box SelectionBox;
    protected readonly Container SelectionContainer;
    protected readonly Circle SelectionEndCircle;
    protected readonly Box Box;

    public TransferBasicSliderBar()
    {
        Masking = true;
        CornerRadius = 10;
        Children =
        [
            Box = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = FrameworkColour.Green,
            },
            SelectionContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    SelectionBox = new Box
                    {
                        Colour = FrameworkColour.Yellow,
                        RelativeSizeAxes = Axes.Both,
                    },
                    SelectionEndCircle = new Circle
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.Centre,
                    }
                ]
            }
        ];
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        // I don't know how I can set size for relative axes size. Because of this use only width and height manually.
        SelectionEndCircle.Size = new Vector2(Height, Height);
    }

    protected override void OnFocus(FocusEvent e)
    {
        updateFocus();
        base.OnFocus(e);
    }

    protected override void OnFocusLost(FocusLostEvent e)
    {
        updateFocus();
        base.OnFocusLost(e);
    }

    protected override void Update()
    {
        if (SelectionEndCircle.Height != Height)
        {
            var circleSize = Height - 1;
            SelectionEndCircle.ResizeTo(new Vector2(circleSize), 200, Easing.OutBack);
        }
    }

    private void updateFocus()
    {
        if (HasFocus)
        {
            BorderThickness = focusBorderThickness;
            BorderColour = FocusColour;
        }
        else
        {
            BorderThickness = 0;
        }
    }

    protected override void UpdateValue(float value)
    {
        SelectionContainer.TransformTo(nameof(Width), value, 300, Easing.OutQuint);
    }
}
