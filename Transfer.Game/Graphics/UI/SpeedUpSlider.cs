using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace Transfer.Game.Graphics.UI
{
    public partial class SpeedUpSlider : TransferBasicSliderBar<double>
    {
        private BindableDouble speedValue;

        protected new readonly Container SelectionBox;

        public SpeedUpSlider()
        {
            BackgroundColour = Colour4.FromHex("#098efc");
            SelectionColour = Colour4.FromHex("#2cb4df");
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.BottomLeft;
            Position = new Vector2(50, 0);
            Height = 10;
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(Size.X/2, Size.Y);
            Masking = true;
            CornerRadius = 5;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            speedValue = new BindableDouble{
                MaxValue = 10.0,
                MinValue = 0.5,
                Value = 1
            };
            Current = speedValue;
        }

        protected override void OnUserChange(double value)
        {
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            Current.Value += e.ScrollDelta.Y;
            if (e.ScrollDelta.X != 0) Current.Value += e.ScrollDelta.X*2;
            return base.OnScroll(e);
        }

        protected override void OnFocus(FocusEvent e)
        {
            Handle(e);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            Handle(e);
        }
    }
}
