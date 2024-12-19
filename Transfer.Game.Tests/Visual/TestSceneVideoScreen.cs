using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Audio.Track;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Configuration;
using Transfer.Game.IO;
using Transfer.Game.Screens;
using Transfer.Game.Graphics.Videos;
using System.IO;
using osu.Framework.Logging;
using System.Linq;
using osu.Framework;
using System.Collections.Generic;
using Transfer.Game.Testing;

namespace Transfer.Game.Tests.Visual
{
    /// <summary>
    /// Removed because it was causing dependency caching problems
    /// </summary>
    [TestFixture]
    public partial class TestSceneVideoScreen : TransferTestScene
    {
        private VideoTestingByte video;
        private string tempFilePath;

        private byte[] targetVideo;

        [BackgroundDependencyLoader]
        private void load(VideoTestingByte resource)
        {
            video = resource;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddAssert("Video", () => video != null, "Video store added");
            AddStep("Check store", () =>
            {
                if (video == null)
                    Logger.Log("ResourceStore is not initialized", level: LogLevel.Error);
                else
                    Logger.Log("ResourceStore is initialized", level: LogLevel.Verbose);
            });

            AddStep("View Videos content", () =>
            {
                targetVideo = video.Data;
            });
            
            AddStep("Writing video bytes to a file", () => {
                byte[] videoInByteArray;
                videoInByteArray = targetVideo;
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
