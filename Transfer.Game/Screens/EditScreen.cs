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

namespace Transfer.Game.Screens;

public partial class EditScreen : TransferScreen
{
    private BoxButton gifConvertButton, mpFConvertButton, aacConvertButton;
    private VideoContainer transferVideo;

    private TempStore<Track> tempStore;
    [Resolved]
    private Storage audioTempStorage { get; set; }

    private string pathToFile;

    public EditScreen() { }
    public EditScreen(VideoContainer video) => transferVideo = video;

    public EditScreen(params object[] args)
    {
        if(args[0] is string){
            string tempPath = args[0].ToString();
            if(!tempPath.StartsWith(!RuntimeInfo.IsUnix ? '\\' : '/'))
                throw new ArgumentNullException("Edit Screen: first arguments is not path to file.");
            pathToFile = tempPath;
        }
        if(args.Contains(transferVideo)){
            foreach(object findVideo in args){
                if(findVideo is VideoContainer videoContainer)
                {
                    transferVideo = videoContainer;
                }
            }
        }

    }

    [BackgroundDependencyLoader]
    private async void load(ExplorerContainer explorerContainer, AudioManager audioManager)
    {
        tempStore = new TempStore<Track>();
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


    }

    protected override void LoadComplete()
    {

        base.LoadComplete();
    }
}
