using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK.Platform.Windows;

namespace Transfer.Game.Graphics.UI;

public partial class ToolsButton(IconUsage icon, Action<SpriteText> text = null) : IconButton(text)
{
    protected override IconUsage? Icon { get; } = icon;
}
