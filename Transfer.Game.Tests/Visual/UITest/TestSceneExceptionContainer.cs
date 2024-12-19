using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using Transfer.Game.Extensions;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Tests.Visual.UITest;

public partial class TestSceneExceptionContainer : TransferTestScene
{
    private ExceptionContainer exceptionContainer;

    public TestSceneExceptionContainer()
    {
        exceptionContainer = new ExceptionContainer
        {
            RelativeSizeAxes = Axes.Both,
            Font = new FontUsage(TransferFonts.FiraCodeNerdFont, size: 30),
            Text = "sdfkgskbfsgdfsdfhsdfbgsd s sggsd sdfdhbgdkjhbg \nf hjsdfb jghbd gshjdfb gjhsdfbg hjskdbsjdhfbghjdb gjh j  bfdhgbdj",
            HeaderText = "Suck"
        };
    }


    [BackgroundDependencyLoader]
    private void load()
    {
        AddUntilStep("Check disposed", () => exceptionContainer.IsAlive);
        AddStep("Add container", () => Add(exceptionContainer));
        AddStep("Create container", () => exceptionContainer = new ExceptionContainer{
            RelativeSizeAxes = Axes.Both,
            Font = new FontUsage(TransferFonts.FiraCodeNerdFont, size: 30),
            Text = "sdfkgskbfsgdfsdfhsdfbgsd s sggsd sdfdhbgdkjhbg \nf hjsdfb jghbd gshjdfb gjhsdfbg hjskdbsjdhfbghjdb gjh j  bfdhgbdj",
            HeaderText = "Suck"
        });
    }






}
