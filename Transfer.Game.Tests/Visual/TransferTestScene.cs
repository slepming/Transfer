using osu.Framework.Testing;

namespace Transfer.Game.Tests.Visual
{
    public abstract partial class TransferTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new TransferTestSceneTestRunner();

        private partial class TransferTestSceneTestRunner : TransferGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
