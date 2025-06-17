using NUnit.Framework;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI.Containers.TestMenuWindow;

public partial class TestSceneOpenPlaylistWindow : TransferTestScene
{
    private OpenPlaylistMenu openPlaylistMenu;

    [Test]
    public void CreatePlaylistWindow()
    {
        Add(openPlaylistMenu = new OpenPlaylistMenu()
        {
            Title = "Open Playlist/Create Playlist"
        });
        AddStep("Change visible", () =>
        {
            if (openPlaylistMenu.Visible)
                openPlaylistMenu.Hide();
            else
                openPlaylistMenu.Show();
        });
    }
}
