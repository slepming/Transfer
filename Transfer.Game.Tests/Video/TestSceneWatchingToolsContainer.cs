using System;
using System.IO;
using NUnit.Framework;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.Video;

public partial class TestSceneWatchingToolsContainer : TransferTestScene
{
    private VideoContainer videoContainer;
    private VideoTestingByte videoTestBytes;
    private string tempFilePath;

    private byte[] targetVideo;

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
            if (videoTestBytes == null) throw new ArgumentNullException();

            targetVideo = videoTestBytes.Data;
            var videoInByteArray = targetVideo;
            tempFilePath = Path.Combine(Path.GetTempPath(), "test_video.mp4");
            File.WriteAllBytes(tempFilePath, videoInByteArray);
            videoContainer = new VideoContainer(tempFilePath)
            {
                RelativeSizeAxes = Axes.Both,
            };
            Add(new WatchingToolsContainer
            {
                Video = videoContainer.GetVideoController()
            });
        });
    }
}
