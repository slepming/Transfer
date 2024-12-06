using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers;

namespace Transfer.Game.Tests.Visual.UITest
{
    public partial class TestSceneConfigurationContainer : TransferTestScene
    {
        private ConfigurationContainer configuration = new ConfigurationContainer();
        public TestSceneConfigurationContainer()
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddUntilStep("Is visible", () => configuration.Visible);
            AddStep("Show configuration", () =>
            {
                configuration.Expire();
                Add(configuration = new ConfigurationContainer());
                configuration.Show();
            });

        }
    }
}
