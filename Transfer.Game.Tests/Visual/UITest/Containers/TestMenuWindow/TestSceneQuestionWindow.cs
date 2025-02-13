using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers.Windows;

namespace Transfer.Game.Tests.Visual.UITest.Containers.TestMenuWindow;

public partial class TestSceneQuestionWindow : TransferTestScene
{
    private QuestionWindow testQuestionWindow;

    public TestSceneQuestionWindow()
    {
        Add(testQuestionWindow = new QuestionWindow()
        {
            QuestionText = "Can I use the ```ls``` command in Linux to view the contents of a directory?"
        });
        testQuestionWindow.Answer += answer;
    }

    private void answer(bool obj)
    {
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddStep("Change visible menu", () =>
        {
            if (testQuestionWindow.Visible)
                testQuestionWindow.Hide();
            else
                testQuestionWindow.Show();
        });
    }
}
