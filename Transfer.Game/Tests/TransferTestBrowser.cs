using osu.Framework.Platform;
using osu.Framework.Testing;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.Tests
{
    public partial class TransferTestBrowser : TransferGameBase
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(new GlobalActionContainer(this)
            {
                Child = new TestBrowser("Transfer")
            });
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            host.Window.CursorState |= CursorState.Hidden;
        }
    }
}
