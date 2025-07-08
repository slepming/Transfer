using System.Reflection;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

public partial class AssemblyInfoDialog : TransferDialog
{
    private Assembly assembly = Assembly.GetExecutingAssembly();
    private TextFlowContainer textFlowContainer;

    public AssemblyInfoDialog()
    {
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
