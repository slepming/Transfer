using NUnit.Framework;
using osu.Framework.Screens;
using Transfer.Game.Screens;

namespace Transfer.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneVideoScreen : TransferTestScene
    {
       public TestSceneVideoScreen()
       {
            Add(new ScreenStack(new VideoScreen()) { RelativeSizeAxes = osu.Framework.Graphics.Axes.Both});
      }
     }
}
