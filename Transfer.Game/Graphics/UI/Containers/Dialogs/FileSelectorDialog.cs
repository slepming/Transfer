using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UIv2;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

public partial class FileSelectorDialog(ICanUpdateVideo screen) : TransferDialog
{
    private TransferFileSelector fileSelector;

    [BackgroundDependencyLoader]
    private void load()
    {
        WindowContent.Add(fileSelector = new TransferFileSelector(null, TransferGameBase.VIDEO_EXTENSIONS)
        {
            RelativeSizeAxes = Axes.Both
        });
        HeaderName = "File selector";
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        fileSelector.CurrentFile.BindValueChanged(f =>
        {
            if (f.NewValue.Exists)
            {
                Hide();
                screen.UpdateVideo(f.NewValue.FullName);
            }
        });
    }
}
