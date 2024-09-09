using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Screens;
using Transfer.Game.Screens;

namespace Transfer.Game.Tests.Visual.UITest
{
    [TestFixture]
    public partial class TestSceneAuthorScreen : TransferTestScene
    {
        public TestSceneAuthorScreen()
        {
            Add(new ScreenStack(new AuthorScreen()) { RelativeSizeAxes = osu.Framework.Graphics.Axes.Both }) ;
        }

    }
}
