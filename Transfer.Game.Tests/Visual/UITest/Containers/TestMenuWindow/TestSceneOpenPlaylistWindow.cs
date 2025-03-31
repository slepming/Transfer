using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers.Windows;

namespace Transfer.Game.Tests.Visual.UITest.Containers.TestMenuWindow;

public partial class TestSceneOpenPlaylistWindow : TransferTestScene
{
    private OpenPlaylistWindow openPlaylistWindow;

    [BackgroundDependencyLoader]
    private void load()
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
