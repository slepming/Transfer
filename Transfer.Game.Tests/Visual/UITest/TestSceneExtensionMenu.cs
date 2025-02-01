using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers.Menu;

namespace Transfer.Game.Tests.Visual.UITest;

public partial class TestSceneExtensionMenu : TransferTestScene
{
    private ExtensionMenu extensionMenu = null!;

    protected override void LoadComplete()
    {
        base.LoadComplete();
        AddStep("Create extension menu", () => extensionMenu = new ExtensionMenu());
        AddAssert("Extension menu is nut null", () => extensionMenu != null);
        AddStep("Adding object on screen", () => Add(extensionMenu));
        AddStep("Change extension menu visible", () =>
        {
            if (extensionMenu.Visible) extensionMenu.Hide();
            else extensionMenu.Show();
        });
    }
}
