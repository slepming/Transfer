using osu.Framework.Graphics.Containers;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI;

public partial class TestSceneExtensionMenu : TransferTestScene
{
    private ExtensionMenu extensionMenu = null!;

    protected override void LoadComplete()
    {
        base.LoadComplete();
        AddStep("Create extension menu", () =>
        {
            if (extensionMenu == null)
            {
                extensionMenu = new ExtensionMenu();
            }
            else
            {
                extensionMenu.Expire();
                extensionMenu = new ExtensionMenu();
            }
        });
        AddAssert("Extension menu is nut null", () => extensionMenu != null);
        AddStep("Adding object on screen", () => Add(extensionMenu));
        AddStep("Change extension menu visible", () =>
        {
            if (extensionMenu.State.Value == Visibility.Visible) extensionMenu.Hide();
            else extensionMenu.Show();
        });
    }
}
