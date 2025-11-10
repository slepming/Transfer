using NUnit.Framework;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI;

public partial class TestSceneTransferTextBox : TransferTestScene
{
    private TransferTextBox box;

    [SetUp]
    private void setUp()
    {
        Clear();
        Add(box = new TransferTextBox(true) { Width = 300, Height = 120, Anchor = Anchor.Centre, Origin = Anchor.Centre });
    }
}
