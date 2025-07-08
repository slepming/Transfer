using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.Cursor
{
    public partial class TransferCursorContainer : CursorContainer
    {
        public TransferCursorContainer()
        {
        }

        protected override Drawable CreateCursor() => new TransferCursor();
    }

    public partial class TransferCursor : CompositeDrawable
    {
        private readonly Container circle;

        public override bool PropagatePositionalInputSubTree => true;

        public TransferCursor()
        {
            Size = new Vector2(30);

            Origin = Anchor.Centre;

            InternalChildren =
            [
                circle = new CircularContainer
                {
                    Size = new Vector2(8),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,
                    BorderThickness = 2,
                    BorderColour = Color4.White,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.GreenYellow
                    }
                }
            ];
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            updateScaleCursor(e);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            updateScaleCursor(e);
            base.OnMouseUp(e);
        }

        private void updateScaleCursor(MouseButtonEvent e)
        {
            circle.ScaleTo(e.CurrentState.Mouse.Buttons.Any() ? 1.2f : 1, 100);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            circle.MoveTo(circle.Position - e.ScrollDelta * 2).MoveTo(Vector2.Zero, 300, Easing.Out);
            return base.OnScroll(e);
        }
    }
}
