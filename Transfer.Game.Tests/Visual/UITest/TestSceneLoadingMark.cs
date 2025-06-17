using Transfer.Game.Graphics.UI.Containers.Overlays;

namespace Transfer.Game.Tests.Visual.UITest
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
