using System.IO;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Logging;
using osu.Framework.Screens;
using Transfer.Game.IO;
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

        [Resolved]
        private TempStorage storage { get; set; }

        [SetUp]
        public void Initialize()
        {
            Logger.Log("Clear cache files", level: LogLevel.Important);
            storage.GetFiles("").ForEach(storage.Delete);
            Scheduler.Add(() => {
                if (Resources != null)
                    videoTestBytes = new VideoTestingByte(Resources.Get(@"Videos/SampleVideoWithAudio.mp4"));
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
