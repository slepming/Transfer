using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using Transfer.Game.Graphics.UI.Containers;
using osu.Framework.Logging;

namespace Transfer.Game.Tests.Visual.UITest.DirContainer;

public partial class TestSceneDirectory : TransferTestScene
{
    private ExplorerContainer testDirectoryContainer;


    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        Add(new Container{
            RelativeSizeAxes = Axes.Both,
            Children = [
                new Sprite
                {
                    Texture = textureStore.Get("Sky"),
                    Width = 1600,
                    Height = 900,
                    Colour = Colour4.White
                },
                testDirectoryContainer = new ExplorerContainer{

                },
            ]
        });
        testDirectoryContainer.TransitionFile += delegate (string path, bool isFile) { Logger.Log(path); };
        testDirectoryContainer.Show();
    }
}
