using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Screens;
using Transfer.Game.Screens;
using Transfer.Game.Testing;

namespace Transfer.Game.Tests.Visual
{
    public partial class TestSceneEditScreen : TransferTestScene
    {
        private VideoTestingByte videoTestBytes;
        private byte[] targetVideo;
        private string tempFilePath;


        [BackgroundDependencyLoader]
        private void load(VideoTestingByte videoBytes)
        {
            videoTestBytes = videoBytes;
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

            AddStep("Writing video bytes to a file", () => {
                byte[] videoInByteArray;
                videoInByteArray = targetVideo;
                tempFilePath = Path.Combine(Path.GetTempPath(), "test_video.mp4");
                File.WriteAllBytes(tempFilePath, videoInByteArray);
            });

            AddStep("Move to Edit Screen", () => {
                Add(new ScreenStack(new EditScreen(tempFilePath)){
                    RelativeSizeAxes = Axes.Both,
                });
            });
        }
    }
}