using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public partial class ChoiceDirectory : Container
    {
        public SpacingText SpacingText;

        public Action<ValueChangedEvent<string>> FoundVideo;
        private DirectoryContainer directoryContainer;
        private DirectoryFillFlowContainer flowContainer;
        public ChoiceDirectory()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {

                directoryContainer = new DirectoryContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1f/2f, 2f/3f),
                    Children = new Drawable[]
                    {
                        SpacingText = new SpacingText
                        {
                            Text = "Choose file",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.Centre,
                            Position = new Vector2(0,20),
                            Font = new FontUsage("FiraCodeNerdFont-Light",size: 40),
                        },

                        new BasicScrollContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(1, 0.7f),
                            Colour = Colour4.White,

                            Child = flowContainer = new DirectoryFillFlowContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Full,
                                Spacing = new Vector2(20, 20),
                            }
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            directoryContainer.Show();
            flowContainer.FoundVideo += FoundVideo;
            base.LoadComplete();
        }
    }
}
