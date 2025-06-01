using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using Transfer.Game.Graphics.UI.Containers.Dialogs;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

public partial class PlaylistMenu : TransferDialog
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

    public Action<string> SelectedFileAction;

    public PlaylistMenu(Storage playlistStorage)
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

    protected override void Update()
    {
        loadStorage(PlaylistStorage);
        base.Update();
    }

    private void loadStorage(Storage storage, TextureStore textureStore = null)
    {
        mainContainer.Clear();
        var files = storage.GetFiles("");

        foreach (var file in files)
        {
            if (!TransferGameBase.VIDEO_EXTENSIONS.Contains(Path.GetExtension(file))) continue;

            mainContainer.Add(new PlaylistButton()
            {
                Height = 25,
                RelativeSizeAxes = Axes.X,
                Current = file,
                Action = onActionExplorerButton
            });
        }
    }

    private void onActionExplorerButton(string path)
    {
        Logger.Log($"User selected file {path}", level: LogLevel.Debug);
        SelectedFileAction?.Invoke(path);
    }
}
