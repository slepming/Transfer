using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ConfigurationContainer : FocusedOverlayContainer, IKeyBindingHandler<GlobalAction>
{
    private bool isHide;
    public ConfigurationContainer()
    {
        isHide = true;
        Hide();
        InternalChildren = new Drawable[]
        {
            new SpriteText{
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Text = "Options",
                Font = new FontUsage("Oswald", size: 50, fixedWidth: true)
            },

        };
    }

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        Logger.Log(e.Action.ToString());
        if(e.Repeat)
            return false;
        switch(e.Action){
            case GlobalAction.ConfigurationMenu:
                if(isHide)
                {
                    Show();
                    isHide = false;
                }
                else{
                    Hide();
                    isHide = true;
                }
                return true;

        }
        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }

    protected override void PopIn()
    {

    }

    protected override void PopOut()
    {

    }
}
