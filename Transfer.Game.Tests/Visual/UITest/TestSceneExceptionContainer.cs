using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Tests.Visual.UITest;

public partial class TestSceneExceptionContainer : TransferTestScene
{
    private ExceptionContainer exceptionContainer;

    private string text =
        "Linux is a family of Unix-like operating systems based on the Linux kernel, including a set of utilities and programs from the GNU project, and possibly other components. Like the Linux kernel, systems based on it are created and distributed in accordance with the free and open-source software development model. Linux systems are mainly distributed for free in the form of various distributions, ready for installation, convenient for maintenance and updates, and having their own set of system and application components, both free and proprietary.";

    public TestSceneExceptionContainer()
    {
        exceptionContainer = new ExceptionContainer
        {
            RelativeSizeAxes = Axes.Both,
            Font = new FontUsage(TransferFonts.FiraCodeNerdFont, size: 20),
            HeaderText = "Suck",
            Text = text
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddUntilStep("Check disposed", () => exceptionContainer.IsAlive);
        AddStep("Add container", () => Add(exceptionContainer));
        AddStep("Create container", () => exceptionContainer = new ExceptionContainer
        {
            RelativeSizeAxes = Axes.Both,
            Font = new FontUsage(TransferFonts.FiraCodeNerdFont, size: 20),
            Text = text,
            HeaderText = "Suck"
        });
    }






}
