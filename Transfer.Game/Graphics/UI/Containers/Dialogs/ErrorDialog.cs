using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

public partial class ErrorDialog : TransferDialog
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

    public ErrorDialog()
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
