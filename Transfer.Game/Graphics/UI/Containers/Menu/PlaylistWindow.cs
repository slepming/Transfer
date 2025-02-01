using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using Transfer.Game.Graphics.UI.Containers.Overlays;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

public partial class PlaylistWindow : Container
{
    private TransparentButton close, changeState;
    private FillFlowContainer mainContainer;

    /// <summary>
    /// If state false when playlist window in Extension menu
    /// </summary>
    public bool State = false;

    protected Storage playlistStorage;

    [Resolved]
    private TransferGameBase gameBase { get; set; }

    public PlaylistWindow(Storage playlistStorage)
    {
        this.playlistStorage = playlistStorage;
        Masking = true;
        BorderThickness = 1;
        BorderColour = Colour4.White;
        InternalChildren =
        [
            close = new TransparentButton
            {
                Width = 30,
                Height = 30,
                Text = "X",
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Action = onCloseButton
            },
            new TransferScrollContainer()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1, 1f / 1.25f),
                Child =
                    mainContainer = new FillFlowContainer()
                    {
                        Spacing = new Vector2(5, 15),
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical
                    }
            }
        ];
    }

    private void onCloseButton()
    {
        Hide();
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        loadStorage(playlistStorage, textureStore);
    }

    private void loadStorage(Storage storage, TextureStore textureStore = null)
    {
        var files = storage.GetFiles("./");

        foreach (var file in files)
        {
            if (!TransferGameBase.VIDEO_EXTENSIONS.Contains(Path.GetExtension(file))) continue;

            mainContainer.Add(new PrefixButton
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Text = file,
                Prefix = textureStore?.Get("video-camera.png"),
                // Action = onActionExplorerButton
            });
        }
    }

    private void onActionExplorerButton(string path, bool isDirectory)
    {
    }
}
