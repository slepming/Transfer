using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;

namespace Transfer.Game.Graphics.UI;

public partial class ToolsButton : IconButton
{
    protected override IconUsage? Icon { get; }

    public ToolsButton(LocalisableString buttonText)
        : base(buttonText)
    {
    }

    public ToolsButton(IconUsage icon)
        : base("")
    {
        Icon = icon;
    }

    public ToolsButton(IconUsage icon, LocalisableString text)
        : base(text)
    {
        Icon = icon;
    }
}
