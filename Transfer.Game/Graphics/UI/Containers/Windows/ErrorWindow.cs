using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Framework.Graphics;

namespace Transfer.Game.Graphics.UI.Containers.Windows;

public partial class ErrorWindow : MenuWindow
{
    public LocalisableString Title
    {
        set
        {
            if (HeaderName == value) return;
            HeaderName = value;
        }
    }

    public LocalisableString Message
    {
        get => message;
        set
        {
            if (message == value) return;
            message = value;
        }
    }

    private LocalisableString message;

    public ErrorWindow()
    {
        BorderColour = Colour4.Red;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        WindowContent.AddRange(
        [
            new TextFlowContainer()
            {
                Text = message,
                RelativeSizeAxes = Axes.Both,
            }
        ]);
    }
}
