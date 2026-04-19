using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI;

public partial class TestSceneExceptionContainer : TransferTestScene
{
    private ErrorDialog errorContainer;

    private string text =
        "Linux is a family of Unix-like operating systems based on the Linux kernel, including a set of utilities and programs from the GNU project, and possibly other components. Like the Linux kernel, systems based on it are created and distributed in accordance with the free and open-source software development model. Linux systems are mainly distributed for free in the form of various distributions, ready for installation, convenient for maintenance and updates, and having their own set of system and application components, both free and proprietary.";

    public TestSceneExceptionContainer()
    {
        errorContainer = new ErrorDialog()
        {
            Title = "Oh god, it's error",
            Message = text
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddStep("Add container", () => Add(errorContainer));
        AddStep("Change Visible", () =>
        {
            if (errorContainer.State.Value == Visibility.Visible)
                errorContainer.Hide();
            else
                errorContainer.Show();
        });
    }






}
