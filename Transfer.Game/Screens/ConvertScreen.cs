using System;
using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI;
using osu.Framework.Graphics;

namespace Transfer.Game.Screens;

public partial class ConvertScreen : TransferScreen
{
    private BoxButton gifConvertButton, mpFConvertButton, aacConvertButton;
    public ConvertScreen() { }
    public ConvertScreen(params string[] args)
    {

    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = [
            gifConvertButton = new BoxButton(){
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(0.2f,0.2f),
                Text = "Gif convert",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            },
        ];
    }

    protected override void LoadComplete()
    {

        base.LoadComplete();
    }
}
