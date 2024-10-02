using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using Vector2 = osuTK.Vector2;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public partial class SpacingText : SpriteText
    {
        public int SpacingEffect = 5;
        public int SpacingEffectLost = 0;


        protected override void LoadComplete()
        {

            base.LoadComplete();
        }
        protected override bool OnHover(HoverEvent e)
        {
            this.TransformSpacingTo(new Vector2(SpacingEffect,0),2000, Easing.OutElastic);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.TransformSpacingTo(new Vector2(SpacingEffectLost,0),2000, Easing.OutElastic);
            base.OnHoverLost(e);
        }
    }
}
