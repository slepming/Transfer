using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Testing;
using Transfer.Game.IO;

namespace Transfer.Game.Tests.Visual;

public abstract partial class TransferTestScene : TestScene
{
    protected IResourceStore<byte[]> Resources;
    protected new DependencyContainer Dependencies { get; private set; }
    protected override ITestSceneTestRunner CreateRunner() => new TransferTestSceneTestRunner();

    protected PlaylistStorage PlaylistStorage;

    protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
    {
        var host = parent.Get<GameHost>() ?? throw new NullReferenceException("Host not found");
        Resources = parent.Get<TransferGameBase>().Resources;

        if (Resources == null)
        {
            throw new NullReferenceException("Resources not found");
        }

        Dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        if (Dependencies == null)
        {
            throw new NullReferenceException("Dependencies not found");
        }

        if (!host.Storage.ExistsDirectory("TestPlaylist"))
            Directory.CreateDirectory(Path.Combine(host.Storage.GetFullPath(string.Empty), "TestPlaylist"));

        Dependencies.Cache(PlaylistStorage = new PlaylistStorage(host.Storage, "TestPlaylist"));

        return Dependencies;
    }

    protected override void LoadComplete()
    {
        ChangeBackgroundColour(Colour4.FromHex("#010221"));
    }

    private partial class TransferTestSceneTestRunner : TransferGameBase, ITestSceneTestRunner
    {
        private TestSceneTestRunner.TestRunner runner;

        protected override void LoadAsyncComplete()
        {
            Add(runner = new TestSceneTestRunner.TestRunner());
        }

        public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
    }

}
