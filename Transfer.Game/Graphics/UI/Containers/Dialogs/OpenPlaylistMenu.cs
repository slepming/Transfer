using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Graphics.UI.Buttons;
using Transfer.Game.IO;

namespace Transfer.Game.Graphics.UI.Containers.Dialogs;

public partial class OpenPlaylistMenu : TransferDialog
{
    private TransferTextBox nameTextBox;
    private StringButton applyButton;

    [Resolved]
    private GameHost host { get; set; }

    public Bindable<PlaylistStorage> Playlist { get; } = new Bindable<PlaylistStorage>();

    public LocalisableString Title;

    [BackgroundDependencyLoader]
    private void load()
    {
        HeaderName = Title;
        WindowContent.AddRange(
            [
                nameTextBox = new TransferTextBox(handleNonPositionalInput: true)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 150,
                    Height = 50,
                    PlaceholderText = "Enter a name(or path)",
                    FontSize = 20
                },
                applyButton = new StringButton("If there is no playlist, the app will create one")
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Width = 75,
                    Height = 25,
                    CornerRadius = 5,
                    Text = "Apply",
                    Action = onApplyAction
                }
            ]
        );
    }

    private void onApplyAction()
    {
        if (string.IsNullOrEmpty(nameTextBox.Current.Value))
            return;

        if (Directory.Exists(nameTextBox.Current.Value))
        {
            Playlist.Value = new PlaylistStorage(new NativeStorage(nameTextBox.Current.Value, null));
            Logger.Log($"Playlist storage exists: {Playlist.Value.GetFullPath("")}");
            this.Hide();
            return;
        }

        string pathToPlaylist = !Path.IsPathFullyQualified(nameTextBox.Current.Value) ? Path.Combine(Path.GetTempPath(), nameTextBox.Current.Value) : nameTextBox.Current.Value;

        Playlist.Value = new PlaylistStorage(new NativeStorage(pathToPlaylist, null));
        Logger.Log($"Playlist has been created at {pathToPlaylist}");
        this.Hide();
    }
}
