using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

public partial class AssemblyInfoDialog : TransferDialog
{
    private readonly Assembly assembly = Assembly.GetExecutingAssembly();

    [BackgroundDependencyLoader]
    private void load()
    {
        TextFlowContainer textFlowContainer;
        HeaderName = "Assembly Info";
        WindowContent.AddRange([
            textFlowContainer = new TextFlowContainer()
            {
                Text = $"{assembly.GetName()}\n",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both
            }
        ]);
#if DEBUG
        textFlowContainer.AddText("Debug Version");
#endif
    }
}
