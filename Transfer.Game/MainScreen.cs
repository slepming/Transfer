using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;

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
