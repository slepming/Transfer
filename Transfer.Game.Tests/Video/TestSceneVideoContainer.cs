using System.IO;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.Video;

[TestFixture]
public partial class TestSceneVideoContainer : TransferTestScene
{
    private VideoContainer videoContainer;
    private VideoTestingByte videoTestBytes;
    private string tempFilePath;

    private byte[] targetVideo;
    private int seekSpace;

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager configManager)
    {
        seekSpace = configManager.Get<int>(TransferOptions.SeekValue);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        if (Resources != null)
        {
            videoTestBytes = new VideoTestingByte(Resources.Get(@"Videos/SampleVideoWithAudio.mp4"));
        }
    }

    [Test]
    public void Initialize()
    {
        AddStep("Load", () =>
        {
            targetVideo = videoTestBytes.Data;
            var videoInByteArray = targetVideo;
            tempFilePath = Path.Combine(Path.GetTempPath(), "test_video.mp4");
            File.WriteAllBytes(tempFilePath, videoInByteArray);
            Clear();
            videoContainer = new VideoContainer(tempFilePath)
            {
                RelativeSizeAxes = Axes.Both,
                SeekSpace = seekSpace,
            };
            Add(videoContainer);
        });
    }

    [Test]
    public void Seek()
    {
        AddStep("Seek", () => videoContainer.Seek(seekSpace));
    }

    [Test]
    public void Player()
    {
        AddStep("Pause/Start", () => videoContainer.Pause());
    }
}
