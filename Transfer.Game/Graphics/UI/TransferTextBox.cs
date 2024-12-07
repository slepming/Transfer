using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.UI;
public partial class TransferTextBox : TextBox, IHasTooltip
{
    public LocalisableString TooltipText => "Acceleration factor.";
    private const float caret_move_time = 100f;
    
    private Box background;

    private Color4 backgroundColour = Color4.Gray.Opacity(100);
    public Color4 BackgroundColour
    {
        get => backgroundColour;
        set => backgroundColour = value;
    }
    public TransferTextBox()
    {
        Masking = true;
        BorderThickness = 3;
        BorderColour = Colour4.White;
        CornerRadius = 5;
        Add(background = new Box{
            RelativeSizeAxes = Axes.Both,
            Depth = 1,
            Colour = backgroundColour
        });
    }
    protected override Caret CreateCaret() => new TransferCaret(){
        CaretWidth = 2,
    };

    protected override SpriteText CreatePlaceholder()
    {
        return new SpriteText(){
            Text = "Enter speed",
            Font = new FontUsage(size: 20, family: "Oswald"),
        };
    }

    protected override void NotifyInputError()
    {
        
    }

    protected override Drawable GetDrawableCharacter(char c) => new ZoomableContainer()
    {
        AutoSizeAxes = Axes.Both,
        Child = new SpriteText { Text = c.ToString(), Font = new FontUsage(family: "Oswald", size: 30)}
    };

    protected override bool CanAddCharacter(char character)
    {
        return !char.IsLetter(character);
    }

    private partial class ZoomableContainer : Container
    {
        public override void Show()
        {
            var col = (Color4)Colour;
            this.ScaleTo(1.5f, caret_move_time, Easing.Out).FadeColour(col.Opacity(0)).FadeColour(col, caret_move_time * 2, Easing.Out);;
        }
        public override void Hide()
        {
            this.ScaleTo(0, caret_move_time, Easing.InQuad);
        }
    }
    private partial class TransferCaret : Caret
    {
        public float CaretWidth { get; set; }
        public TransferCaret()
        {
            Colour = Colour4.White;
            InternalChild = new Container
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.Both,
                Height = 0.9f,
                CornerRadius = 1f,
                Masking = true,
                Child = new Box { RelativeSizeAxes = Axes.Both },
            };
        }

        public override void Hide() => this.FadeOut(400);
        public override void DisplayAt(Vector2 position, float? selectionWidth)
        {
            if(selectionWidth != null)
            {
                this.MoveTo(position, 50, Easing.Out);
                this
                    .FadeTo(0.5f, 200, Easing.Out)
                    .FadeColour(Colour4.White, 200, Easing.Out);
            }
            else{
                this.MoveTo(new Vector2(position.X - CaretWidth/2), 50, Easing.Out);
            }
        }
    }
}
