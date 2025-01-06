using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

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

            InternalChildren = new Drawable[]
            {

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
                },
            };
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    circle.ScaleTo(1.2f, 200);
                    break;

                case MouseButton.Right:
                    circle.ScaleTo(1.2f, 200);
                    break;
            }

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    circle.ScaleTo(1f, 100);
                    break;

                case MouseButton.Right:
                    circle.ScaleTo(1f, 100);
                    break;
            }

            base.OnMouseUp(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            circle.MoveTo(circle.Position - e.ScrollDelta * 2).MoveTo(Vector2.Zero, 300, Easing.Out);
            return base.OnScroll(e);
        }

    }
}
