using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Graphics.Cursor;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerContainer : TransferFocusedOverlayContainer
{
    protected override bool BlockScrollInput { get; }
    protected override bool StartHidden { get; }
    protected override bool BlockPositionalInput { get; }

    private ExplorerFillFlowContainer explorerFillFlowContainer;

    public event TransitionEvent FoundVideo;

    private TransferTextBox searchTextBox;
    


    public ExplorerContainer(bool blockScrollInput = true, bool startHidden = true, bool blockPositionalInput = true)
    {
        Hide();
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
                    Position = new Vector2(0,9),
                    Font = new FontUsage("Oswald",size: 40)
                },
                
                new FinalContextMenuContainer{
                    RelativeSizeAxes = Axes.Both,
                    Child = new TransferScrollContainer{
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
                },
                searchTextBox = new TransferTextBox(true)
                {
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(0.2f, 1),
                    Height = 40,
                    Tooltip = "Search your file",
                    PlaceholderText = "Enter",
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                },

        ];
        searchTextBox.Current.ValueChanged += searchInExplorer;
        explorerFillFlowContainer.TransitionPath += onTransitionPath;
        base.LoadComplete();
    }

    private void onTransitionPath(string path, bool isFile)
    {
        searchTextBox.Text = "";
        FoundVideo?.Invoke(path, isFile);
    }

    private void searchInExplorer(ValueChangedEvent<string> text)
    {
        explorerFillFlowContainer.SearchTerm = text.NewValue;
    }

    protected override void PopIn()
    {
        this.FadeIn(1000, Easing.In).ScaleTo(1, 600, Easing.In);
    }

    protected override void PopOut()
    {
        this.FadeOut(600,Easing.Out).ScaleTo(0.8f);
    }
}
