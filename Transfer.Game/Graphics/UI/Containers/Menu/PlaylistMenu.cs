using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Graphics.UIv2;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.UI.Containers.Menu;

/// <summary>
/// Menu for playlists. Shows a list of videos added to PlaylistStorage.
/// </summary>
/// <remarks>
/// Use this only after load. For example in LoadComplete.
/// </remarks>
public partial class PlaylistMenu : TransferDialog
{
    private TransparentButton close;
    private readonly FillFlowContainer mainContainer;
    private TransferFileSelector fileSelector;

    public LocalisableString Title
    {
        get => HeaderName;
        set
        {
            if (HeaderName.Equals(value)) return;

            HeaderName = value;
        }
    }

    protected PlaylistStorage PlaylistStorage;

    [CanBeNull]
    protected ICanUpdateVideo Screen;

    public Action<string> SelectedFileAction;

    public PlaylistMenu(PlaylistStorage playlistStorage, ICanUpdateVideo screen = null)
    {
        if (playlistStorage == null)
        {
            throw new ArgumentNullException(nameof(playlistStorage));
        }

        Screen = screen;

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
        if (Screen == null)
            loadStorage(PlaylistStorage, textureStore);
        else
            loadStorage(PlaylistStorage, Screen);
    }

    private void loadStorage(PlaylistStorage storage, ICanUpdateVideo screen)
    {
        WindowContent.Clear();
        WindowContent.Add(fileSelector = new TransferFileSelector(storage.GetFullPath(""))
        {
            RelativeSizeAxes = Axes.Both
        });
        fileSelector.CurrentFile.BindValueChanged(f =>
        {
            if (f.NewValue.Exists)
                screen.UpdateVideo(File.ResolveLinkTarget(f.NewValue.FullName, true)?.FullName);
        });
    }

    [Obsolete]
    private void loadStorage(PlaylistStorage storage, TextureStore textureStore = null)
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
