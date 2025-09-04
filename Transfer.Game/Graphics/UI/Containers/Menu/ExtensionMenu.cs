using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

public partial class ExtensionMenu : TransferFocusedOverlayContainer
{
    private StringButton playlistButton, openPlaylistButton;

    private TransferContainer innerHub, menuHub;
    private TransferDialog playlistContainer, openPlaylistWindow;

    private readonly ICanUpdateVideo updateVideo;

    /// <summary>
    /// Extension menu for user playlists
    /// </summary>
    /// <param name="updateVideo">Object implementing <see cref="ICanUpdateVideo"/></param>
    public ExtensionMenu(ICanUpdateVideo updateVideo = null)
    {
        this.updateVideo = updateVideo;
        RelativeSizeAxes = Axes.Both;
        InternalChildren =
        [
            menuHub = new TransferContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.25f, 1),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Masking = true,
                MaskingSmoothness = 3,
                Children =
                [
                    new Box
                    {
                        Colour = Colour4.Gray.Opacity(30),
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 1
                    },
                    innerHub = new TransferContainer
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
                                    Spacing = new Vector2(5, 5),
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
                                        playlistButton = new StringButton
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            Size = new Vector2(1f / 1.1f, 30),
                                            Text = "Go to current playlist",
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            BackgroundColour = Colour4.FromHex("#333333"),
                                            Masking = true,
                                            BorderColour = Colour4.FromHex("#CCCCCC"),
                                            BorderThickness = 2,
                                            CornerRadius = 2,
                                            Colour = Colour4.White,
                                            Action = openPlaylistList
                                        },
                                        openPlaylistButton = new StringButton
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            Size = new Vector2(1f / 1.1f, 30),
                                            Text = "Open playlist",
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            BackgroundColour = Colour4.FromHex("#333333"),
                                            Masking = true,
                                            BorderColour = Colour4.FromHex("#CCCCCC"),
                                            BorderThickness = 2,
                                            CornerRadius = 2,
                                            Colour = Colour4.White,
                                            Action = openPlaylistFromPath
                                        },
                                    ]
                                }
                            }
                        ]
                    }
                ]
            }
        ];
    }

    private void openPlaylistFromPath()
    {
        openPlaylistWindow.Show();
    }

    [BackgroundDependencyLoader]
    private void load(PlaylistStorage playlistStorage)
    {
        playlistContainer = new PlaylistMenu(playlistStorage)
        {
            Title = "Playlist Menu",
            SelectedFileAction = selectedFileAction
        };
        openPlaylistWindow = new OpenPlaylistMenu
        {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Title = "Open Playlist",
        };
        ((OpenPlaylistMenu)openPlaylistWindow).Playlist.ValueChanged += setPlaylist;
        AddRange(new List<TransferDialog> { playlistContainer, openPlaylistWindow });
    }

    private void setPlaylist(ValueChangedEvent<PlaylistStorage> playlistStorage)
    {
        playlistContainer = new PlaylistMenu(playlistStorage.NewValue)
        {
            Title = "Playlist Menu",
            SelectedFileAction = selectedFileAction
        };
    }

    private void selectedFileAction(string path)
    {
        updateVideo?.UpdateVideo(path);
    }

    private void openPlaylistList()
    {
        playlistContainer.Show();
    }

    public override void Hide()
    {
        this.ResizeTo(new Vector2(0, 1), 300, Easing.InCubic);
        base.Hide();
    }

    public override void Show()
    {
        this.ResizeTo(new Vector2(1, 1), 300, Easing.OutCubic);
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
