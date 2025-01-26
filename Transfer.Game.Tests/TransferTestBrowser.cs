using osu.Framework.Allocation;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Testing;
using Transfer.Game.Configuration;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.Testing;

namespace Transfer.Game.Tests
{
    public partial class TransferTestBrowser : TransferGameBase
    {
        private DependencyContainer testDependencies;
        private PlaylistStorage playlistStorage;
        private TransferConfigManager transferConfigManager;

        [BackgroundDependencyLoader]
        private void load()
        {
            transferConfigManager = new TransferConfigManager(Host.Storage);
            testDependencies.Cache(transferConfigManager);
            Logger.Log($"Playlist path: {transferConfigManager.Get<string>(TransferOptions.CurrentPlaylistPath)}");
            playlistStorage = new PlaylistStorage(new NativeStorage(transferConfigManager.Get<string>(TransferOptions.CurrentPlaylistPath)));
            testDependencies.CacheAs(playlistStorage);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            VideoTestingByte videoTestData = new VideoTestingByte(Resources.Get(@"Videos/SampleVideoWithAudio.mp4"));
            testDependencies.CacheAs(videoTestData);

            AddRange(
            [
                new GlobalActionContainer(this)
                {
                    Children =
                    [
                        new TestBrowser("Transfer"),
                        new CursorContainer()
                    ]
                },
            ]);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) => testDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}
