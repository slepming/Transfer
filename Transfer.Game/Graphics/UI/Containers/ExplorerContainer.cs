using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Transfer.Game.UserInterface.DirectoryHandler;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerContainer : FocusedOverlayContainer
{
    protected override bool BlockScrollInput { get; }
    protected override bool StartHidden { get; }
    protected override bool BlockPositionalInput { get; }

    private ExplorerFillFlowContainer explorerFillFlowContainer;

    public event TransitionEvent FoundVideo;

    public ExplorerContainer(bool blockScrollInput = true, bool startHidden = true, bool blockPositionalInput = true)
    {
        Masking = true;
        BlockScrollInput = blockScrollInput;
        StartHidden = startHidden;
        BlockPositionalInput = blockPositionalInput;
        RelativeSizeAxes = Axes.Both;
        Origin = Anchor.Centre;
        Anchor = Anchor.Centre;
        Size = new Vector2(0.9f, 0.9f);
    }

    protected override void LoadComplete()
    {
        InternalChildren = [
                new SpacingText{
                    Text = "Choice file",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = new FontUsage("Oswald",size: 40)
                },
                new ExplorerScrollContainer{
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(0, 50),


                    Child = explorerFillFlowContainer = new ExplorerFillFlowContainer{
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Spacing = new Vector2(15,15),
                    }
                }

            ];
        explorerFillFlowContainer.TransitionPath += FoundVideo;
        base.LoadComplete();
    }
    protected override void PopIn()
    {
        this.FadeTo(1, 1000, Easing.InOutQuint);
    }

    protected override void PopOut()
    {
        this.FadeTo(0.1f, 600, Easing.InOutQuint);
    }
}
