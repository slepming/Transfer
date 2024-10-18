using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Transfer.Game.Screens;
using Transfer.Game.UserInterface;
using Vector2 = osuTK.Vector2;

namespace Transfer.Game
{
    public partial class MainScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {


        }
        protected override void LoadComplete()
        {
            base.LoadComplete();
        }
        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
        }
    }
}
