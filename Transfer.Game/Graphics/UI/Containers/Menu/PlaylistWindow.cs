using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Platform;
using osuTK;
using Transfer.Game.Graphics.UI.Containers.Windows;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

public partial class PlaylistWindow : MenuWindow
{
    private TransparentButton close;
    private FillFlowContainer mainContainer;

    public LocalisableString Title
    {
        get => HeaderName;
        set
        {
            if (HeaderName == value) return;
            HeaderName = value;
        }
    }

    protected Storage PlaylistStorage;

    public PlaylistWindow(Storage playlistStorage)
    {
        this.PlaylistStorage = playlistStorage;
        WindowContent.AddRange(
        [
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
        ]);
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textureStore)
    {
        loadStorage(PlaylistStorage, textureStore);
    }

    private void loadStorage(Storage storage, TextureStore textureStore = null)
    {
        var files = storage.GetFiles("");

        foreach (var file in files)
        {
            if (!TransferGameBase.VIDEO_EXTENSIONS.Contains(Path.GetExtension(file))) continue;

            mainContainer.Add(new PrefixButton
            {
                Height = 25,
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
