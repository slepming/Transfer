using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UIv2;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI;

public partial class TestSceneTransferFileSelector : TransferTestScene
{
    [BackgroundDependencyLoader]
    private void load()
    {
        Add(new TransferFileSelector(null, TransferGameBase.VIDEO_EXTENSIONS)
        {
            RelativeSizeAxes = Axes.Both,
        });
    }
}
