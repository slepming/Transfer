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
    public partial class MainScreen : TransferScreen
    {
        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            // Texture backgroundButtonIcon = textureStore.Get("settings");
            // if(backgroundButtonIcon == null) Logger.Error(new Exception(),"Background Button texture icon is null");
            // InternalChildren = new Drawable[]
            // {

            //     new Sprite
            //     {
            //         Texture = backgroundButtonIcon,
            //         Size = new Vector2(30,30),
            //         Anchor = Anchor.Centre,
            //         Origin = Anchor.Centre
            //     }
            // };

        }
        protected override void LoadComplete()
        {
            Logger.Log("📺 MainScreen loading");
            this.Push(new VideoScreen());
            base.LoadAsyncComplete();
        }
        public override void OnEntering(ScreenTransitionEvent e)
        {
            Logger.Log($"Entered in {Description}");
            base.OnEntering(e);
        }
    }
}
