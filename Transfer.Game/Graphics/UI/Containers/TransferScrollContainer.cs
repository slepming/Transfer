using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class TransferScrollContainer(Direction direction = Direction.Vertical) : TransferScrollContainer<Drawable>(direction);

public partial class TransferScrollContainer<T>(Direction direction) : ScrollContainer<T>(direction)
    where T : Drawable
{
    protected override ScrollbarContainer CreateScrollbar(Direction direction) => new ScrollBar(direction);

    protected partial class ScrollBar : ScrollbarContainer
    {
        private const float dim_size = 0;
        private readonly Box scrollBox;

        public ScrollBar(Direction direction)
            : base(direction)
        {
            Child = scrollBox = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Transparent
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            scrollBox.FadeColour(Colour4.White, 1000, Easing.InOutQuart);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            scrollBox.FadeColour(Colour4.Transparent, 500, Easing.InOutCubic);
            base.OnHoverLost(e);
        }

        public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
        {
            Vector2 size = new Vector2(dim_size)
            {
                [(int)ScrollDirection] = val
            };
            this.ResizeTo(size, duration, easing);
        }
    }
}
