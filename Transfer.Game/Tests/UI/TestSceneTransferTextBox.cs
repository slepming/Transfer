using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
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
        Add(new TooltipContainer()
        {
            RelativeSizeAxes = Axes.Both,
            Child = box = new TransferTextBox(true) { Width = 300, Height = 60, Anchor = Anchor.Centre, Origin = Anchor.Centre }
        });
    }
}
