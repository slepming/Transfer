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
    [Resolved]
    private PlaylistStorage playlist { get; set; }

    private PlaylistMenu playlistMenu;

    private string currentFileName;
    private readonly string standartPath = Path.Combine(Path.GetTempPath(), "TestVideo");

    [BackgroundDependencyLoader]
    private void load()
    {
        playlistMenu = new PlaylistMenu(playlist)
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

    [Test]
    public void TestPlaylistWriting()
    {
        AddStep("Write to playlist storage", () =>
        {
            if (!Directory.Exists(standartPath)) Directory.CreateDirectory(standartPath);
            currentFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".mp4";
            File.Create(Path.Combine(standartPath, currentFileName)).Dispose();
            if (playlist.Exists(Path.GetFileName(Path.Combine(standartPath, currentFileName))))
                playlist.Delete(Path.GetFileName(Path.Combine(standartPath, currentFileName)));
            playlist.SaveDataFromPathToLink(Path.Combine(standartPath, currentFileName));
        });
        AddStep("Delete all playlist files", () =>
        {
            playlist.DeleteAll();
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
    }
}
