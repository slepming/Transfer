using System.IO;
using NUnit.Framework;
using osu.Framework.Logging;
using osu.Framework.Screens;
using Transfer.Game.Screens;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.Video
{
    [TestFixture]
    public partial class TestSceneVideoScreen : TransferTestScene
    {
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
                targetVideo = videoTestBytes.Data;

                var videoInByteArray = targetVideo;
                tempFilePath = Path.Combine(Path.GetTempPath(), "test_video.mp4");
                File.WriteAllBytes(tempFilePath, videoInByteArray);
                Add(new ScreenStack(new VideoScreen(tempFilePath))
                {
                    RelativeSizeAxes = osu.Framework.Graphics.Axes.Both,
                });
            });
        }
    }
}
