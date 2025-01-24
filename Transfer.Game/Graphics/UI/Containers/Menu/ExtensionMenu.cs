using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

public partial class ExtensionMenu : TransferFocusedOverlayContainer
{
    private BasicButton explorerButton, playlistButton, optionsButton;
    private Container hub;
    private TransferScrollContainer playlistContainer;
    private FillFlowContainer playlists;

    public ExtensionMenu()
    {
        RelativeSizeAxes = Axes.Both;
        Size = new Vector2(0.25f, 1);
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.CentreLeft;
        Masking = true;
        MaskingSmoothness = 3;
        InternalChildren =
        [
            new Box
            {
                Colour = Colour4.Gray.Opacity(30),
                RelativeSizeAxes = Axes.Both,
                Alpha = 1
            },
            hub = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    new SpriteText // header
                    {
                        Text = "Choose menu",
                        Font = new FontUsage(family: TransferFonts.Oswald, size: 45),
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                    new TransferScrollContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(1, 1f / 1.25f),
                        ScrollbarVisible = true,
                        Child = new FillFlowContainer()
                        {
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Spacing = new Vector2(5,5),
                            Children =
                            [
                                //explorerButton = new BasicButton
                                //{
                                //    RelativeSizeAxes = Axes.X,
                                //    Height = 30,
                                //    Size = new Vector2(1 / 1.1f, 30),
                                //    Text = "Go to explorer",
                                //    Anchor = Anchor.Centre,
                                //    Origin = Anchor.Centre
                                //},
                                playlistButton = new BasicButton
                                {
                                    RelativeSizeAxes = Axes.X,
                                    Height = 30,
                                    Size = new Vector2(1 / 1.1f, 30),
                                    Text = "Go to current playlist",
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    BackgroundColour = Colour4.FromHex("#333333"),
                                    Masking = true,
                                    BorderColour = Colour4.FromHex("#CCCCCC"),
                                    BorderThickness = 2,
                                    Colour = Colour4.White,
                                    CornerRadius = 5,
                                }
                            ]
                        }
                    }
                ]
            }
        ];

        playlistButton.Action += openPlaylist;
    }

    [BackgroundDependencyLoader]
    private void load(PlaylistStorage playlistStorage)
    {
        playlistContainer = new TransferScrollContainer
        {
            RelativeSizeAxes = Axes.Both,
            Child = playlists = new PlaylistWindow(playlistStorage)
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            }
        };
        playlistContainer.Hide();
        Add(playlistContainer);
    }

    private void openPlaylist()
    {
        hub.Hide();
        playlistContainer.Show();
    }

    public override void Hide()
    {
        this.ResizeTo(new Vector2(0, 1), 300, Easing.InCubic);
        base.Hide();
    }

    public override void Show()
    {
        this.ResizeTo(new Vector2(0.25f, 1), 300, Easing.OutCubic);
        base.Show();
    }

    protected override void PopIn()
    {
        this.FadeIn(200);
    }

    protected override void PopOut()
    {
        this.FadeOut(200);
    }
}
