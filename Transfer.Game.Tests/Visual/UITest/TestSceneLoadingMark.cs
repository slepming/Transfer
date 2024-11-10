using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transfer.Game.Graphics.UI.Containers;

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
