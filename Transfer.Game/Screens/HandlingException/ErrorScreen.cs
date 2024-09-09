using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace Transfer.Game.Screens
{
    public partial class ErrorScreen : TransferScreen
    {
        public ErrorScreen()
        {
            
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.Centre,
                    Colour = Colour4.DarkGoldenrod,
                    Child = new SpriteText
                    {
                        Text = "Error, please support developer",
                        Colour = Colour4.GhostWhite,
                    }
                }
            };
        }
    }
}