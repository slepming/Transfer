using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI;

public partial class TestSceneConfigurationContainer : TransferTestScene
{
    private ConfigurationContainer configuration = new ConfigurationContainer();

    public TestSceneConfigurationContainer()
    {
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddStep("Show configuration", () =>
        {
            configuration.Expire();
            Add(configuration = new ConfigurationContainer());
            configuration.Show();
        });
    }
}
