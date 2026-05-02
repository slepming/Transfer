using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.IO;

namespace Transfer.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneTransferGame : TransferTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private TransferGame game;

        [Resolved]
        private TempStorage storage { get; set; }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new TransferGame();
            game.SetHost(host);

            AddGame(game);
        }

        [Test]
        public void Cache()
        {
            AddStep("Clear cache", () => {
                Logger.Log("Clear cache files", level: LogLevel.Important);
                storage.GetFiles("").ForEach(storage.Delete);
            });
        }
    }
}
