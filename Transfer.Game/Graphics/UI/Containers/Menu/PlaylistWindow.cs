using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osuTK;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

public partial class PlaylistWindow : FillFlowContainer
{
    private TransparentButton close, changeState;

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
        Direction = FillDirection.Vertical;
        Spacing = new Vector2(5, 15);
        InternalChildren =
        [
            close = new TransparentButton
            {
                Width = 30,
                Height = 30,
                Text = "X",
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            }
        ];
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        loadStorage(playlistStorage);
    }

    private void loadStorage(Storage storage)
    {
        var files = storage.GetFiles("./");

        foreach (var file in files)
        {
            if (!TransferGameBase.VIDEO_EXTENSIONS.Contains(Path.GetExtension(file))) continue;

            Add(new Explorer.ExplorerButton()
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Text = file,
                Action = onActionExplorerButton
            });
        }
    }

    private void onActionExplorerButton(string path, bool isDirectory)
    {
        // Крч нужно оптимизировать дело смены видео. Каждый раз создавать новый Screen не варик, слишком много ОЗУ ест.

    }
}
