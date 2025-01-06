using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Screens;
using osuTK.Graphics;
using Transfer.Game.Extensions;
using Transfer.Game.Screens;

namespace Transfer.Game.Graphics.UI;

public delegate void TransitionEvent(string path, bool isFile);
public partial class ExplorerButton : ClickableContainer, IHasContextMenu, IFilterable
{
    /// <summary>
    /// Keep text for button text. Can be null.
    /// </summary>
    private string text;
    public string Text {
        get => text;
        set {
            if (value == text) return;
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

    [Resolved]
    private ScreenStack screenStack { get; set; }

    public MenuItem[] ContextMenuItems =>
    [
        new MenuItem("Move to Edit", moveToEditScreen)
    ];

    public bool MatchingFilter { set => this.FadeTo(value ? 1 : 0); }
    public bool FilteringActive { get; set; }

    public IEnumerable<LocalisableString> FilterTerms
    {
        get
        {
            if(path != null) yield return System.IO.Path.GetFileName(path);
        }
    }

    private SpriteText spriteText;
    private Box background;
    public Colour4 TextColour = Colour4.White;

    public event TransitionEvent Transition;
    public ExplorerButton()
    {
        Masking = true;
        RelativeSizeAxes = Axes.X;
        Height = 50;
        BorderColour = Colour4.Gray;
        BorderThickness = 2;
        CornerRadius = 5;
        InternalChildren = [
            background = new Box{
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.3f,
                Colour = Colour4.White,
            },
            spriteText = new SpriteText{
                Colour = TextColour,
                Origin =  Anchor.Centre,
                Anchor = Anchor.Centre,
                Font = new FontUsage(TransferFonts.Oswald,size: 30)
            }
        ];
        Action += pathTransition;
    }

    private void pathTransition()
    {
        if (path == null) return;
        Transition?.Invoke(path, false);

    }
    private void moveToEditScreen()
    {
        if (File.Exists(Path) && TransferGameBase.VIDEO_EXTENSIONS.Contains(System.IO.Path.GetExtension(Path)))
        {
            screenStack.Push(new EditScreen(Path));
        }
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        spriteText.Text = text ?? (System.IO.Path.GetFileName(path));
    }

    protected override bool OnHover(HoverEvent e)
    {
        background.FadeColour(Colour4.Gray, 600, Easing.OutQuint);
        this.TransformTo(nameof(BorderColour), ColourInfo.SingleColour(Color4.Black), 300, Easing.InOutQuad);
        return base.OnHover(e);
    }
    protected override void OnHoverLost(HoverLostEvent e)
    {
        background.FadeColour(Colour4.White, 100, Easing.OutQuint);
        this.TransformTo(nameof(BorderColour), ColourInfo.SingleColour(Color4.Gray), 150, Easing.InOutQuad);
        base.OnHoverLost(e);
    }

    
}
