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
using osu.Framework.Logging;
using osu.Framework.Graphics.Shapes;
using Transfer.Game.UserInterface;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Cursor;
using System.Collections.Generic;
using Transfer.Game.Audio.Extensions;
using Transfer.Game.Audio;
using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Screens;
using osu.Framework.Graphics.Sprites;

namespace Transfer.Game.Screens;

public partial class EditScreen : TransferScreen
{
    private string pathToFile;
    private Dictionary<VideoEditingFunction, string> ffmpegFunctions = new Dictionary<VideoEditingFunction,string>();
    private BoxButton confirmButton, videoAcceleration;

    private TransferTextBox textBox;

    private VideoContainer transferVideo;

    private AudioExtract<Track> tempStore;

    private ExplorerContainer explorerContainer = new();

    

    [Resolved]
    private Storage audioTempStorage { get; set; }

    [Resolved]
    private TransferConfigManager transferConfigManager{ get; set; }

    public EditScreen() { }
    public EditScreen(VideoContainer video) => transferVideo = video;
    public EditScreen(string pathToFile) => this.pathToFile = pathToFile;

    [BackgroundDependencyLoader]
    private async void load(AudioManager audioManager)
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
                Size = new Vector2(Size.X/2, Size.Y/2),
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Audio = await tempStore.CreateaAndGetTrackAsync(pathToFile, audioTempStorage, audioManager),
                WatchingToolsVisible = false,
            };

            
        }
        confirmButton = new BoxButton{
            Text = "Confirm",
            Width = 100,
            Height = 50,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,

        };
        videoAcceleration = new BoxButton{
            Text = "Acceleration",
            Width = 100,
            Height = 50,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            Position = new Vector2(confirmButton.Width + 10, 0)
        };
        videoAcceleration.Action += accelerationVideoButtonAction;
        confirmButton.Action += confirmVideoEditingButtonAction;

        textBox = new TransferTextBox
        {
            Width = 100,
            Height = 30,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            Position = new Vector2(videoAcceleration.X, -50),
            LengthLimit = 4,
            Tooltip = "Acceleration factor.",
            CanAddCharacters = false,
            PlaceholderText = "Enter speed."
        };

        textBox.Hide();
        textBox.OnCommit += commitAccelerationTextBox;

        Box background = new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.DimGray,
            Alpha = 0.3f
        };
        
        Scheduler.AddDelayed(() => {
            InternalChildren = [
                background,
                new TooltipContainer{
                    RelativeSizeAxes = Axes.Both,
                    Child = textBox,
                },
                transferVideo,
                confirmButton,
                videoAcceleration
            ];
        }, 500);
    }

    private void commitAccelerationTextBox(TextBox sender, bool newText)
    {
        float factor = float.TryParse(sender.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float value) ? value : 0f;
        if(factor > 100 || factor < 0.5f) NotifyError.Value = "The factor cannot be above 100 and below 0.5";
        float modifiedFactor = 1/factor;

        if (ffmpegFunctions.TryGetValue(VideoEditingFunction.VideoAcceleration, out _)) {
            ffmpegFunctions[VideoEditingFunction.VideoAcceleration] = FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoAcceleration, modifiedFactor.ToString(CultureInfo.InvariantCulture));
            ffmpegFunctions.Add(VideoEditingFunction.AudioAcceleration, FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.AudioAcceleration, factor.ToString(CultureInfo.InvariantCulture)));
            return;
        }
        ffmpegFunctions.Add(VideoEditingFunction.VideoAcceleration,FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoAcceleration, modifiedFactor.ToString(CultureInfo.InvariantCulture)));
        ffmpegFunctions.Add(VideoEditingFunction.AudioAcceleration, FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.AudioAcceleration, factor.ToString(CultureInfo.InvariantCulture)));
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
    }


    private bool textBoxVisible = false;
    private void accelerationVideoButtonAction()
    {
        textBoxVisible = !textBoxVisible;
        if (textBoxVisible) textBox.Hide();
        else textBox.Show();

    }
    private async void confirmVideoEditingButtonAction()
    {
        string arguments = string.Join(" ", ffmpegFunctions.Values);
        if(string.IsNullOrEmpty(arguments)) return;
        string path = Path.Combine(transferConfigManager.Get<string>(TransferOptions.OutputPath), Path.GetFileNameWithoutExtension(pathToFile) + "_modified" + Path.GetExtension(pathToFile));
        if(File.Exists(path)) path = Path.Combine(transferConfigManager.Get<string>(TransferOptions.OutputPath), Path.GetFileNameWithoutExtension(pathToFile) + $"_{DateTime.Now.ToShortTimeString()}{Path.GetExtension(pathToFile)}");
        Logger.Log($"File create in {path} with arguments {arguments}");
        ClearInternal();
        LoadingOverlay loadingStatus;
        InternalChild = loadingStatus  = new LoadingOverlay()
        {
            Header = "Asking FFmpeg for help..."
        };
        TransferFFmpegCore.CONVERSION_STATUS.ValueChanged += (e) => loadingStatus.Header = e.NewValue;
        await TransferFFmpegCore.Conversion(pathToFile, transferConfigManager, path, arguments);
        ClearInternal();
        InternalChild = new SpriteText{
            Text = $"Done, you can open the video at path {path}",
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Font = new FontUsage("FiraCodeNerdFont-Bold")
        };
        Scheduler.AddDelayed(() => this.Push(new VideoScreen()), 2000);
    }
}
