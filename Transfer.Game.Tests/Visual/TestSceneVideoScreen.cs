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

namespace Transfer.Game.Tests.Visual
{
    /// <summary>
    /// Removed because it was causing dependency caching problems
    /// </summary>
    [TestFixture]
    public partial class TestSceneVideoScreen : TransferTestScene
    {
        private TransferVideo transferVideo;
        private ResourceStore<byte[]> videoStore;
        [BackgroundDependencyLoader]
        private void load(ResourceStore<byte[]> videos)
        {
            videoStore = videos;
            
        }


        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddAssert("Video store", () => videoStore == null ? false : true, "Video store added");
            
            AddStep("Writing video bytes to a file", () => {
                byte[] videoInByteArray;
                string tempFilePath;
                videoInByteArray = videoStore.Get("TestVideo.mp4");
                tempFilePath = Path.Combine(Path.GetTempPath(), "test_video.mp4");
                File.WriteAllBytes(tempFilePath, videoInByteArray);
                transferVideo = new TransferVideo(tempFilePath);
            });
            AddAssert("File added", () => transferVideo == null ? false : true);        
            AddStep("Push to a new screen", () => Add(new ScreenStack(new VideoScreen())
                    {
                        RelativeSizeAxes = osu.Framework.Graphics.Axes.Both,

                    }));
        }
    }
}
