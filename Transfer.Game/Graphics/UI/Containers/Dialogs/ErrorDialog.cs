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
            if (HeaderName.Equals(value)) return;

            HeaderName = value;
        }
    }

    public LocalisableString Message
    {
        get => message;
        set
        {
            if (message.Equals(value)) return;

            message = value;
        }
    }

    private LocalisableString message;

    protected override bool StartHidden => false;

    public ErrorDialog()
    {
        // I don't know why, but framework set visible to hidden
        State.Value = Visibility.Visible;
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

    protected override void PopOut()
    {
        base.PopOut();
    }
}
