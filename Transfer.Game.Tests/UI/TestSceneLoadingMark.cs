using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI
{
    public partial class TestSceneLoadingMark : TransferTestScene
    {
        private LoadingOverlay loadingOverlay;

        public TestSceneLoadingMark()
        {
            Add(loadingOverlay = new LoadingOverlay());
            loadingOverlay.Show();
        }
    }
}
