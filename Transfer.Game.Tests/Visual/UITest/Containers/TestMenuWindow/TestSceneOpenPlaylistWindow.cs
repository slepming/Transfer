using NUnit.Framework;
using Transfer.Game.Graphics.UI.Containers.Windows;

namespace Transfer.Game.Tests.Visual.UITest.Containers.TestMenuWindow;

public partial class TestSceneOpenPlaylistWindow : TransferTestScene
{
    private OpenPlaylistWindow openPlaylistWindow;

    [Test]
    public void CreatePlaylistWindow()
    {
        Add(openPlaylistWindow = new OpenPlaylistWindow()
        {
            Title = "Open Playlist/Create Playlist"
        });
        AddStep("Change visible", () =>
        {
            if (openPlaylistWindow.Visible)
                openPlaylistWindow.Hide();
            else
                openPlaylistWindow.Show();
        });
    }
}
