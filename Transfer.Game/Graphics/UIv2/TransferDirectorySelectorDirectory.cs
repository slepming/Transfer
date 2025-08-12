using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferDirectorySelectorDirectory(DirectoryInfo directory, string displayName = null) : DirectorySelectorDirectory(directory, displayName)
{
    protected override IconUsage? Icon => Directory.Name.Contains(Path.DirectorySeparatorChar) ? FontAwesome.Solid.Database : FontAwesome.Regular.Folder;

    protected override SpriteText CreateSpriteText() => new SpriteText
    {
        Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont)
    };
}
