using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Transfer.Game.UserInterface;

namespace Transfer.Game.Tests.Visual.UITest
{
    public partial class TestSceneLoading : TransferTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new LoadingCircle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new osuTK.Vector2(50,50)
                });
        }
    }
}
