using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics;

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
