using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers;

namespace Transfer.Game.Tests.Visual.UITest;

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
