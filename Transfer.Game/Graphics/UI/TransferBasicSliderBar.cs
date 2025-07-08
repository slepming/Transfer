using System.Numerics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.UI;

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
            if (value == focusBorderThickness) return;
            focusBorderThickness = value;
        }
    }

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
                        Width = 10,
                        Height = 10,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.Centre,
                    }
                ]
            }
        ];

        Masking = true;
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
        SelectionContainer.TransformTo(nameof(Width), value, 300 , Easing.OutQuint);
    }

    protected override void OnUserChange(T value)
    {
        this.FlashColour(ColourInfo.GradientVertical(Color4.Pink, Color4.HotPink), 200, Easing.In);
    }
}
