using System;

namespace KIO.Game.UserInterface.Buttons
{
    public partial class KIOMenuBaseButton : MenuButton
    {
        public KIOMenuBaseButton(string text, Action action) : base(text, action)
        {
            Size = new osuTK.Vector2(150,200);
        }
    }
}