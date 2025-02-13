using System.Reflection;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Transfer.Game.Graphics.UI.Containers.Windows;

public partial class AssemblyInfoWindow : MenuWindow
{
    private Assembly assembly = Assembly.GetExecutingAssembly();

    public AssemblyInfoWindow()
    {
        HeaderName = "Assembly Info";
        WindowContent.AddRange([
            new TextFlowContainer()
            {
                Text = $"Version {assembly.GetName()}",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both
            }
        ]);
    }
}
