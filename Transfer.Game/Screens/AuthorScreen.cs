using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using Transfer.Game.UserInterface;

namespace Transfer.Game.Screens
{
    public partial class AuthorScreen : TransferScreen
    {
        private LoadingCircle loadingCircle;
        private Sprite background;
        public AuthorScreen()
        {

            InternalChildren = new Drawable[]
            {

                background = new Sprite
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(2f,2),
                },
                loadingCircle = new LoadingCircle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
            };

        }
        [BackgroundDependencyLoader]
        private void load(TextureStore texture)
        {
            background.Texture = texture.Get("Sky");
        }

        protected override void LoadComplete()
        {
            loadingCircle.RotateTo(360,1000, Easing.OutQuad).Then().MoveToOffset(new Vector2(-30,30), 3000, Easing.OutCubic).Then().MoveToOffset(new Vector2(-100,200), 2000, Easing.OutElastic).Then().MoveToOffset(new Vector2(-5000,-230), 10000, Easing.OutCubic).Then().Expire();
            background.ResizeTo(new Vector2(2,2), 1000).Then().ResizeTo(new Vector2(2,2), 3000).Then().ResizeTo(new Vector2(2,2),2000).Then().MoveTo(new Vector2(ScreenSpaceDrawQuad.Centre.X-120, 0), 2000, Easing.InOutQuad).Then().ResizeTo(new Vector2(1,1), 1000, Easing.InOutExpo);


            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch(e.Key)
            {
                case osuTK.Input.Key.R: this.Push(new AuthorScreen()); break;
            }
            return base.OnKeyDown(e);
        }

    }
}
