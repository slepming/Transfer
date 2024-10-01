using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions;
using osuTK;
using osu.Framework.Localisation;
using FFmpeg.NET.Extensions;

namespace Transfer.Game.UserInterface.Containers;

public partial class ExceptionContainer : FocusedOverlayContainer
{

    private Container container;
    private ClickableContainer exceptionButton;

    protected override bool BlockScrollInput { get; }
    protected override bool StartHidden { get; }
    protected override bool BlockPositionalInput { get; }
    protected override bool BlockNonPositionalInput => false;


    public TextFlowContainer ContentObject;
    public SpriteText Header;


    private string text;
    public string Text
    {
        get => text;
        set {
            if(value == text) return;
            text = value;
        }
    }
    private string headerText;
    public string HeaderText
    {
        get => headerText;
        set {
            if(value == headerText) return;
            headerText = value;
        }
    }

    public FontUsage Font { get; set; } = new FontUsage(family: "FiraCodeNerdFont",size: 30);





    public ExceptionContainer(bool blockScrollInput = true, bool startHidden = true, bool blockPositionalInput = true)
    {
        Show();


        BlockScrollInput = blockScrollInput;
        StartHidden = startHidden;
        BlockPositionalInput = blockPositionalInput;
        Masking = true;
        BorderColour = Colour4.White;
        CornerRadius = 1;
        BorderThickness = 1;
        Colour = Colour4.White;
        Anchor = Anchor.TopCentre;
        Origin = Anchor.TopCentre;
        Width = 1f;
        Height = 0.2f;
        Scale = new Vector2(1,1);

        InternalChildren =
        [

            container = new Container{
                RelativeSizeAxes = Axes.Both,
                Children = [
                    new Box{
                        Colour = new Colour4(255,255,255,0.01f),
                        RelativeSizeAxes = Axes.Both,

                    },
                    Header = new SpriteText{
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = new FontUsage(family: "FiraCodeNerdFont",size: 50, fixedWidth: true),
                        Text = "Null Header",
                        Colour = Colour4.Red
                    },
                    ContentObject = new TextFlowContainer{
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Position = new Vector2(0,40),
                        RelativeSizeAxes = Axes.Both,
                        Width = 1,
                        Height = 0.4f,
                        Colour = Colour4.White,
                        TextAnchor = Anchor.Centre,
                        Masking = true,

                    },
                    exceptionButton = new ExceptionButton
                    {
                        Text = "Close",
                        TextColour = Colour4.White,
                        BackgroundColour = new Colour4(255,255,255,1),
                        Width = 125,
                        Height = 50,
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                    }
            ]
            }
        ];
        exceptionButton.Action += buttonActionOnClick;

    }

    private void buttonActionOnClick()
    {
        this.TransformTo(nameof(Width), 0.1f, 400, Easing.InOutQuint).Expire();
    }

    [BackgroundDependencyLoader]
    private void load()
    {
    }
    protected override void LoadComplete()
    {
        ContentObject.Text = "";
        ContentObject.AddText(text);
        Header.Text = HeaderText;
        base.LoadComplete();
    }

    public void AddText(LocalisableString localisableString)
    {
        ContentObject.AddText(localisableString);
    }
    public void ReplaceText(LocalisableString localisableString)
    {
        ContentObject.Text = localisableString;
    }

    protected override bool OnHover(HoverEvent e)
    {
        this.TransformTo(nameof(BorderThickness), 2f, 400, Easing.InOutQuad);
        this.TransformTo(nameof(CornerRadius), 10f, 500, Easing.InOutQuint);
        return base.OnHover(e);
    }
    protected override void OnHoverLost(HoverLostEvent e)
    {
        this.TransformTo(nameof(BorderThickness), 1f, 200, Easing.InOutQuad);
        this.TransformTo(nameof(CornerRadius), 5f, 400, Easing.InOutQuint);
        base.OnHoverLost(e);
    }


    protected override void PopIn()
    {
        this.TransformTo(nameof(Width), 1f, 1000, Easing.InOutQuint);
    }

    protected override void PopOut()
    {
        this.TransformTo(nameof(Width), 0f, 200, Easing.InOutQuint);
    }


}
