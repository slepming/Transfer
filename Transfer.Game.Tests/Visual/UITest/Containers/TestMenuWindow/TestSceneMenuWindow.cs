using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UI.Containers.Windows;

namespace Transfer.Game.Tests.Visual.UITest.Containers.TestMenuWindow;

public partial class TestSceneMenuWindow : TransferTestScene
{
    private MenuWindow testMenuWindow;

    public TestSceneMenuWindow()
    {
        Add(testMenuWindow = new TestMenuWindow());
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddStep("Change visible menu", () =>
        {
            if (testMenuWindow.Visible)
                testMenuWindow.Hide();
            else
                testMenuWindow.Show();
        });
    }

    internal partial class TestMenuWindow : MenuWindow
    {
        public TestMenuWindow()
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
