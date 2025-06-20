using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.Cursor;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerContainer : VisibilityContainer
{
    private Transfer.Game.Graphics.UIv2.Containers.ExplorerFillFlowContainer explorerFillFlowContainer;

    public event TransitionEvent FoundVideo;

    private TransferTextBox searchTextBox;

    public ExplorerContainer()
    {
        Hide();
        Masking = true;
        RelativeSizeAxes = Axes.Both;
        Origin = Anchor.Centre;
        Anchor = Anchor.Centre;
        Size = new Vector2(0.9f, 0.9f);
    }

    protected override void LoadComplete()
    {
        InternalChildren =
        [
            new SpacingText
            {
                Text = "Select file",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Position = new Vector2(0, 9),
                Font = new FontUsage(TransferFonts.Oswald, size: 40)
            },

            new FinalContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new TransferScrollContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(0, 50),

                    Child = explorerFillFlowContainer = new UIv2.Containers.ExplorerFillFlowContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(0.25f, 0),
                        AutoSizeAxes = Axes.Y,
                        Spacing = new Vector2(15, 15),
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
        explorerFillFlowContainer.Action += onTransitionPath;
        base.LoadComplete();
    }

    private void onTransitionPath(string path)
    {
        searchTextBox.Text = "";
        FoundVideo?.Invoke(path);
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
        this.FadeOut(600, Easing.Out).ScaleTo(0.8f);
    }
}
