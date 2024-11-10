using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.IO;
using Transfer.Game.Screens;

namespace Transfer.Game.Tests.Visual
{
    /// <summary>
    /// Removed because it was causing dependency caching problems
    /// </summary>
    [TestFixture]
    public partial class TestSceneVideoScreen : TransferTestScene
    {
       

        public TestSceneVideoScreen()
        {
            //Add(new ScreenStack(new VideoScreen()) { RelativeSizeAxes = osu.Framework.Graphics.Axes.Both });
        }
     }
}
