using System.IO;
using NUnit.Framework;
using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.IO;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.IO;

[TestFixture]
public partial class TestScenePlaylist : TransferTestScene
{
    private PlaylistMenu playlistMenu;

    private string currentFileName;
    private readonly string classicPath = Path.Combine(Path.GetTempPath(), "TestVideo");

    [BackgroundDependencyLoader]
    private void load()
    {
        playlistMenu = new PlaylistMenu(PlaylistStorage)
        {
            Title = "TestPlaylist",
        };
    }

    [Test]
    public void CreateTestVideo()
    {
        AddStep("Initialization window", () =>
        {
            Add(playlistMenu);
        });
    }

    private void saveData()
    {
        if (!Directory.Exists(classicPath)) Directory.CreateDirectory(classicPath);
        currentFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".mp4";
        File.Create(Path.Combine(classicPath, currentFileName)).Dispose();
        if (PlaylistStorage.Exists(Path.GetFileName(Path.Combine(classicPath, currentFileName))))
            return;

        PlaylistStorage.SaveDataFromPathToLink(Path.Combine(classicPath, currentFileName));
    }

    [Test]
    public void TestPlaylistWriting()
    {
        AddStep("Write to playlist storage", saveData);
        AddStep("Delete all playlist files", () =>
        {
            PlaylistStorage.DeleteAll();
        });
    }

    [Test]
    public void TestLoadPlaylist()
    {
        AddStep("Open or close playlist list window (auto)", () =>
        {
            if (playlistMenu.Visible)
                playlistMenu.Hide();
            else
                playlistMenu.Show();
        });
        AddStep("Open playlist list window", () => playlistMenu.Show());
        AddStep("Close playlist list window", () => playlistMenu.Hide());
        AddStep("Update playlist", () => playlistMenu.UpdatePath());
    }
}
