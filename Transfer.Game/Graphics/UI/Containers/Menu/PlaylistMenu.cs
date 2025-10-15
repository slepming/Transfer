using System;
using System.IO;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Logging;
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
    private readonly FillFlowContainer mainContainer;
    private PlaylistFileSelector fileSelector;

    public LocalisableString Title
    {
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

        PlaylistStorage = playlistStorage;
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
        loadStorage(PlaylistStorage, Screen);
    }

    private void loadStorage(PlaylistStorage storage, [CanBeNull] ICanUpdateVideo screen)
    {
        WindowContent.Clear();
        WindowContent.Add(fileSelector = new PlaylistFileSelector(storage.GetFullPath(""))
        {
            RelativeSizeAxes = Axes.Both
        });
        fileSelector.CurrentFile.BindValueChanged(f =>
        {
            if (f.NewValue.Exists && screen != null)
            {
                try
                {
                    screen.UpdateVideo(File.ResolveLinkTarget(f.NewValue.FullName, true)?.FullName);
                }
                catch (FileNotFoundException fnfe)
                {
                    Logger.Error(fnfe, "Playlist menu error");
                    OnHandledException.Invoke(fnfe);
                }
            }
        });
    }

    private void onActionExplorerButton(string path)
    {
        Logger.Log($"User selected file {path}", level: LogLevel.Debug);
        SelectedFileAction?.Invoke(path);
    }

    public void UpdatePath() => fileSelector.UpdateFiles();
}
