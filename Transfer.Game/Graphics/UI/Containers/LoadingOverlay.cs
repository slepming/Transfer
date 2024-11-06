using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osuTK;
using Transfer.Game.UserInterface;

namespace Transfer.Game.Graphics.UI.Containers
{
    public partial class LoadingOverlay : OverlayContainer
    {
        protected override bool BlockScrollInput { get; }

        private readonly Loading loadingComponent;
        public LoadingOverlay(bool blockScrollInput = true){
            Show();
            BlockScrollInput = blockScrollInput;
            RelativeSizeAxes = Axes.Both;
            InternalChild = loadingComponent = new Loading{
                Width = 200,
                Height = 200,
            };

        }



        protected override void PopIn()
        {
            this.FadeTo(1,200, Easing.InOutCubic);
        }

        protected override void PopOut()
        {
            this.FadeTo(0, 400, Easing.InOutCubic);
        }

    }
}
