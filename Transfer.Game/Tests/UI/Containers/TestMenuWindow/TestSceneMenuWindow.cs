using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI.Containers.TestMenuWindow;

public partial class TestSceneMenuWindow : TransferTestScene
{
    private TransferDialog testTransferDialog;

    public TestSceneMenuWindow()
    {
        Add(testTransferDialog = new TestTransferDialog());
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddStep("Change visible menu", () =>
        {
            if (testTransferDialog.State.Value == Visibility.Visible)
                testTransferDialog.Hide();
            else
                testTransferDialog.Show();
        });
    }

    private partial class TestTransferDialog : TransferDialog
    {
        public TestTransferDialog()
        {
            HeaderName = "TestMenuWindow";
            WindowContent.Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderColour = Colour4.White,
                BorderThickness = 4,
                CornerRadius = 3,
                Child = new Box
                {
                    Colour = Colour4.White,
                    Alpha = 0.5f,
                    Width = 50,
                    Height = 50,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            });
        }
    }
}
