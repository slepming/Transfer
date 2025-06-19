using System.IO;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Testing;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.Video;

public partial class TestSceneWatchingToolsContainer : TransferTestScene
{
    private VideoContainer videoContainer;
    private VideoTestingByte videoTestBytes;
    private string tempFilePath;

    private byte[] targetVideo;
    private int seekSpace;

    [BackgroundDependencyLoader]
    private void load(VideoTestingByte resource, TransferConfigManager configManager)
    {
        videoTestBytes = resource;
        seekSpace = configManager.Get<int>(TransferOptions.SeekValue);
    }

    [Test]
    public void Initialize()
    {
        AddAssert("Video", () => videoTestBytes != null, "Video store added");
        AddStep("Check bytes", () =>
        {
            if (videoTestBytes == null)
                Logger.Log("Resources is not initialized", level: LogLevel.Error);
            else
                Logger.Log("Resources is initialized", level: LogLevel.Verbose);
        });

        AddStep("View Videos content", () =>
        {
            targetVideo = videoTestBytes.Data;
        });

        AddAssert("Check target video is null or not", () => targetVideo is not null);
        AddStep("Writing video bytes to a file", () =>
        {
            var videoInByteArray = targetVideo;
            tempFilePath = Path.Combine(Path.GetTempPath(), "test_video.mp4");
            File.WriteAllBytes(tempFilePath, videoInByteArray);
        });
        AddStep("Adding container on screen.", () => Add(new WatchingToolsContainer()
        {
            RelativeSizeAxes = Axes.Both,
            Video = videoContainer
        }));
    }
}
