using System.Reflection;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers.Windows;

public partial class AssemblyInfoWindow : MenuWindow
{
    private Assembly assembly = Assembly.GetExecutingAssembly();
    private TextFlowContainer textFlowContainer;

    public AssemblyInfoWindow()
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
