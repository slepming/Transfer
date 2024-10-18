using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.UI;

public delegate void TransitionEvent(string path, bool isFile);
public partial class ExplorerButton : ClickableContainer
{
    /// <summary>
    /// Keep text for button text. Can be null.
    /// </summary>
    private string text;
    public string Text {
        get => text;
        set{
            if(value == text) return;
            text = value;
        }
    }
    private string path;
    public string Path
    {
        get => path;
        set
        {
            if (value == path) return;
            path = value;
        }
    }

    public List<string> AllowedFileExtensions = new List<string>();

    private SpriteText spriteText;
    public Colour4 TextColour = Colour4.White;

    public event TransitionEvent Transition;
    public ExplorerButton()
    {
        Masking = true;
        Width = 100;
        Height = 50;
        AutoSizeAxes = Axes.X;
        BorderColour = Colour4.Gray;
        BorderThickness = 2;
        CornerRadius = 5;
        InternalChildren = [
            new Box{
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.3f,
                Colour = Colour4.White,
            },
            spriteText = new SpriteText{
                Colour = TextColour,
                Origin =  Anchor.Centre,
                Anchor = Anchor.Centre,
                Font = new FontUsage("Oswald",size: 30)

            }
        ];
        Action += pathTransition;
    }

    private void pathTransition()
    {
        if (path == null) return;
        Transition?.Invoke(path, System.IO.Path.GetExtension(path) == null ? true : false);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        spriteText.Text = text ?? (path == null ? "None" : (path.Split(RuntimeInfo.IsUnix ? '/' : '\\').Last() ?? "None"));
    }

    protected override bool OnHover(HoverEvent e)
    {
        this.FadeColour(Colour4.Gray, 600, Easing.OutQuint);
        this.TransformTo(nameof(BorderColour), Color4.Black, 700, Easing.InOutQuad); // crash
        return base.OnHover(e);
    }
    protected override void OnHoverLost(HoverLostEvent e)
    {
        this.FadeColour(Colour4.White, 400, Easing.InOutQuint);
        this.TransformTo(nameof(BorderColour), Color4.Gray, 300, Easing.InOutQuad);
        base.OnHoverLost(e);
    }
}
