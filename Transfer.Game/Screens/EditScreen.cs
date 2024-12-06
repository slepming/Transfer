using System;
using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI;
using osu.Framework.Graphics;
using osu.Framework;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.UserInterface.Containers;
using osuTK;
using System.Linq;
using Transfer.Game.IO;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.Platform;
using Transfer.Game.Configuration;
using System.IO;

namespace Transfer.Game.Screens;

public partial class EditScreen : TransferScreen
{
    private BoxButton convertButton, videoAcceleration;



    private VideoContainer transferVideo;

    private AudioExtract<Track> tempStore;

    [Resolved]
    private Storage audioTempStorage { get; set; }

    private string pathToFile;

    public EditScreen() { }
    public EditScreen(VideoContainer video) => transferVideo = video;
    public EditScreen(string pathToFile) => this.pathToFile = pathToFile;

    [BackgroundDependencyLoader]
    private async void load(ExplorerContainer explorerContainer, AudioManager audioManager, TransferConfigManager transferConfigManager)
    {
        tempStore = new AudioExtract<Track>(transferConfigManager);
        if(transferVideo == null){
            if(pathToFile == null){
                explorerContainer.Show();
                explorerContainer.FoundVideo += (string path, bool isFile) =>
                {
                    pathToFile = path;
                };
            }
            transferVideo = new VideoContainer(pathToFile)
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1/4,1/4),
                Anchor = Anchor.TopRight,
                Origin = Anchor.Centre,
                Audio = await tempStore.CreateaAndGetTrackAsync(pathToFile, audioTempStorage, audioManager)
            };
        }
        convertButton = new BoxButton{
            Text = "Convert",
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,

        };
        videoAcceleration = new BoxButton{
            Text = "Acceleration",
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Position = new Vector2(convertButton.Width + 10, 0)
        };

    }

    protected override void LoadComplete()
    {

        base.LoadComplete();
    }
}
