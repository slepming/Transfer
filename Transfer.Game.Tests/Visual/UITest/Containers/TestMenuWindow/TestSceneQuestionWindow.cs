using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI.Containers.Dialogs;

namespace Transfer.Game.Tests.Visual.UITest.Containers.TestMenuWindow;

public partial class TestSceneQuestionWindow : TransferTestScene
{
    private QuestionDialog testQuestionDialog;

    public TestSceneQuestionWindow()
    {
        Add(testQuestionDialog = new QuestionDialog()
        {
            QuestionText = "Can I use the ```ls``` command in Linux to view the contents of a directory?"
        });
        testQuestionDialog.Answer += answer;
    }

    private void answer(bool obj)
    {
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddStep("Change visible menu", () =>
        {
            if (testQuestionDialog.Visible)
                testQuestionDialog.Hide();
            else
                testQuestionDialog.Show();
        });
    }
}
