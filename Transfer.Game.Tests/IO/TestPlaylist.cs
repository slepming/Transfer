using System.IO;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.IO;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.IO;

[TestFixture]
public partial class TestPlaylist : TransferTestScene
{
    [Resolved]
    private PlaylistStorage playlist { get; set; }

    [Resolved]
    private ResourceStore<byte[]> resources { get; set; }

    private PlaylistWindow playlistWindow;

    private string currentFileName;
    private readonly string standartPath = Path.Combine(Path.GetTempPath(), "TestVideo");

    [BackgroundDependencyLoader]
    private void load()
    {
        playlistWindow = new PlaylistWindow(playlist)
        {
            Title = "TestPlaylist",
        };
    }

    [Test]
    public void CreateTestVideo()
    {
        AddStep("Initialization window", () =>
        {
            Add(playlistWindow);
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
        AddAssert("Check file availability", () => playlist.Exists(Path.GetFileName(Path.Combine(standartPath, currentFileName))));
    }

    [Test]
    public void TestLoadPlaylist()
    {
        AddStep("Open or close playlist list window (auto)", () =>
        {
            if (playlistWindow.Visible)
                playlistWindow.Hide();
            else
                playlistWindow.Show();
        });
        AddStep("Open playlist list window", () => playlistWindow.Show());
        AddStep("Close playlist list window", () => playlistWindow.Hide());
    }
}
