using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace Transfer.Game.Tests.UI
{
    public partial class TestFocusedOverlayContainer : FocusedOverlayContainer
    {
        protected override bool StartHidden { get; }

        protected override bool BlockPositionalInput { get; }

        protected override bool BlockNonPositionalInput => false;

        protected override bool BlockScrollInput { get; }

        public TestFocusedOverlayContainer(bool startHidden = true, bool blockPositionalInput = true, bool blockScrollInput = true)
        {
            Show();

            BlockPositionalInput = blockPositionalInput;
            BlockScrollInput = blockScrollInput;

            StartHidden = startHidden;

            Size = new Vector2(0.5f);
            RelativeSizeAxes = Axes.Both;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Cyan,
                    RelativeSizeAxes = Axes.Both,
                },
                new BasicButton
                {
                    Text = "Click",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(50,50),
                    BackgroundColour = Colour4.White,
                    Colour = Colour4.Black
                }
            };

            State.ValueChanged += _ => FireCount++;
        }

        public int FireCount { get; private set; }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => BlockPositionalInput || base.ReceivePositionalInputAt(screenSpacePos);

        protected override bool OnClick(ClickEvent e)
        {
            if (!base.ReceivePositionalInputAt(e.ScreenSpaceMousePosition))
            {
                Hide();
                return true;
            }

            return false;
        }

        protected override void PopIn()
        {
            this.FadeIn(1000, Easing.OutQuint);
            this.ScaleTo(1, 1000, Easing.OutElastic);
        }

        protected override void PopOut()
        {
            this.FadeOut(1000, Easing.OutQuint);
            this.ScaleTo(0.4f, 1000, Easing.OutQuint);
        }
    }
}
