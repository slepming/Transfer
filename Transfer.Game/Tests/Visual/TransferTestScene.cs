using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Colour;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Testing;
using osuTK.Graphics;

namespace Transfer.Game.Tests.Visual
{
    public abstract partial class TransferTestScene : TestScene
    {
        protected IResourceStore<byte[]> Resources;
        protected new DependencyContainer Dependencies { get; private set; }
        protected override ITestSceneTestRunner CreateRunner() => new TransferTestSceneTestRunner();

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var host = parent.Get<GameHost>();

            if (host == null)
            {
                throw new NullReferenceException("Host not found");
            }

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

            return Dependencies;
        }

        protected override void LoadComplete()
        {
            ChangeBackgroundColour(ColourInfo.GradientVertical(Color4.Black, Color4.DarkGray));
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
}
