using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using Transfer.Game.Graphics.UI;
using Vector2 = osuTK.Vector2;


namespace Transfer.Game.Tests.Visual.UITest
{
    [TestFixture]
    public partial class TestSceneTestDirectoryContainer : TransferTestScene
    {
        public TestSceneTestDirectoryContainer()
        {
            Add(new ScreenStack(new TestDirectoryScene()));
        }

        private partial class TestDirectoryScene : Screen
        {

            [BackgroundDependencyLoader]
            private void load()
            {
                InternalChildren = new Drawable[]
                {
                    new TestDirectoryContainer
                    {
                        RelativeSizeAxes = Axes.Both,

                    }
                };
            }
        }

        private partial class TestDirectoryContainer : CompositeDrawable
        {
            public TestDirectoryContainer()
            {
                AddInternal(new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,

                    Child = new FillFlowContainer<SpacingText>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children = new[]
                        {
                            new SpacingText{ Text = "Im not gay", Font = new FontUsage(size:50)},
                            new SpacingText{ Text = "Im not gay", Font = new FontUsage(size:50)},
                        },

                    }
                });
            }
        }



    }
}
