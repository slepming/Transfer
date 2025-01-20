using Transfer.Game.Graphics.UI.Containers.Overlays;

namespace Transfer.Game.Tests.Visual.UITest
{
    public partial class TestSceneLoadingMark : TransferTestScene
    {
        public TestSceneLoadingMark()
        {
            Add(new LoadingOverlay());
        }
    }
}
