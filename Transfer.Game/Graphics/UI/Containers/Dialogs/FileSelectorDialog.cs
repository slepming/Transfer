using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UIv2;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

public partial class FileSelectorDialog(ICanUpdateVideo screen) : TransferDialog
{
    private readonly ICanUpdateVideo updateVideo = screen;

    private TransferFileSelector fileSelector;

    [BackgroundDependencyLoader]
    private void load()
    {
        WindowContent.Add(fileSelector = new TransferFileSelector(null, TransferGameBase.VIDEO_EXTENSIONS)
        {
            RelativeSizeAxes = Axes.Both
        });
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        fileSelector.CurrentFile.BindValueChanged(f =>
        {
            if (f.NewValue.Exists)
                updateVideo.UpdateVideo(f.NewValue.FullName);
        });
    }
}
