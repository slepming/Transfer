using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osuTK;
using Transfer.Game.Graphics.UI.Containers;
using ExplorerFillFlowContainer = Transfer.Game.Graphics.UI.Explorer.ExplorerFillFlowContainer;

namespace Transfer.Game.Tests.Visual.UITest.DirContainer;

public partial class TestSceneDirectory : TransferTestScene
{
    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        ExplorerFillFlowContainer explorerContainer;
        Add(new TransferScrollContainer
        {
            RelativeSizeAxes = Axes.Both,
            Child = explorerContainer = new ExplorerFillFlowContainer(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            {
                Spacing = new Vector2(20, 20),
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            },
        });
    }
}
