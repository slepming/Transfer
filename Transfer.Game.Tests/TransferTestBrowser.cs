using osu.Framework.Allocation;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;
using Transfer.Game.Input.Bindings;
using Transfer.Game.Testing;

namespace Transfer.Game.Tests
{
    public partial class TransferTestBrowser : TransferGameBase
    {

        private DependencyContainer testDependencies;
        protected override void LoadComplete()
        {
            base.LoadComplete();

            VideoTestingByte videoTestData = new VideoTestingByte(Resources.Get(@"Videos/Priroda.mp4"));
            testDependencies.CacheAs(videoTestData);
            
            AddRange(
            [
                new GlobalActionContainer(this){
                    Children = [
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
