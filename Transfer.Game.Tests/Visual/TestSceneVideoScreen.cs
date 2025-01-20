using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using Transfer.Game.Screens;
using System.IO;
using osu.Framework.Logging;
using Transfer.Game.Testing;

namespace Transfer.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneVideoScreen : TransferTestScene
    {
        private VideoTestingByte videoTestBytes;
        private string tempFilePath;

        private byte[] targetVideo;

        [BackgroundDependencyLoader]
        private void load(VideoTestingByte resource)
        {
            videoTestBytes = resource;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
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
            AddStep("Push to a new screen", () => Add(new ScreenStack(new VideoScreen(tempFilePath))
            {
                RelativeSizeAxes = osu.Framework.Graphics.Axes.Both,
            }));
        }
    }
}
