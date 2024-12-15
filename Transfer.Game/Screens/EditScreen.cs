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
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Cursor;
using System.Collections.Generic;
using Transfer.Game.Audio;
using System.Globalization;
using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace Transfer.Game.Screens;

public partial class EditScreen : TransferScreen
{
    private string pathToFile;
    private string fileName = string.Empty;
    private Dictionary<VideoEditingFunction, string> ffmpegFunctions = new Dictionary<VideoEditingFunction,string>();
    private TransparentButton confirmButton, videoSpeedUp;

    private TransferTextBox speedUpTextBox, videoNameAndExtensionTextBox;

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
        confirmButton = new TransparentButton{
            Text = "Confirm",
            Width = 100,
            Height = 50,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,

        };
        confirmButton.Action += confirmVideoEditingButtonAction;
        videoSpeedUp = new TransparentButton{
            Text = "Acceleration",
            Width = 100,
            Height = 50,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            Position = new Vector2(confirmButton.Width + 10, 0)
        };
        videoSpeedUp.Action += accelerationVideoButtonAction;

        videoNameAndExtensionTextBox = new TransferTextBox(true)
        {
            Width = 100,
            Height = 30,
            Anchor = Anchor.TopLeft,
            Origin = Anchor.TopLeft,
            Tooltip = "You can change the file name here",
            PlaceholderText = "Enter name",
            Depth = 1,
        };

        videoNameAndExtensionTextBox.Current.ValueChanged += onChangeValueInNameTextBox;

        speedUpTextBox = new TransferTextBox(true)
        {
            Width = 100,
            Height = 30,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
            Position = new Vector2(videoSpeedUp.X, -50),
            LengthLimit = 4,
            Tooltip = "Acceleration factor. Press 'Enter' to confirm",
            CanAddCharacters = false,
            PlaceholderText = "Enter speed.",
            Depth = 1
        };

        speedUpTextBox.Hide();
        speedUpTextBox.OnCommit += commitAccelerationTextBox;

        Box background = new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.DimGray,
            Alpha = 0.3f,
            Depth = 0
        };
        
        Scheduler.AddDelayed(() => {
            InternalChildren = [
                new TooltipContainer{
                    RelativeSizeAxes = Axes.Both,
                    Children = [
                        speedUpTextBox,
                        videoNameAndExtensionTextBox
                        ],
                },
                background,
                transferVideo,
                confirmButton,
                videoSpeedUp,
                
            ];
        }, 500);
    }

    private void onChangeValueInNameTextBox(ValueChangedEvent<string> e)
    {
        fileName = e.NewValue;
    }

    private void commitAccelerationTextBox(TextBox sender, bool newText)
    {
        float factor = float.TryParse(sender.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out float value) ? value : 0f;
        if (factor == 1) return;
        if(factor > 100 || factor < 0.5f) NotifyError.Value = "The factor cannot be above 100 and below 0.5";
        float modifiedFactor = 1/factor;

        if (ffmpegFunctions.TryGetValue(VideoEditingFunction.VideoSpeedUp, out _)) {
            ffmpegFunctions[VideoEditingFunction.VideoSpeedUp] = FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoSpeedUp, modifiedFactor.ToString(CultureInfo.InvariantCulture));
            ffmpegFunctions[VideoEditingFunction.AudioSpeedUp] = FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.AudioSpeedUp, factor.ToString(CultureInfo.InvariantCulture));
            return;
        }
        ffmpegFunctions.Add(VideoEditingFunction.VideoSpeedUp,FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoSpeedUp, modifiedFactor.ToString(CultureInfo.InvariantCulture)));
        ffmpegFunctions.Add(VideoEditingFunction.AudioSpeedUp, FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.AudioSpeedUp, factor.ToString(CultureInfo.InvariantCulture)));
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
    }


    private bool speedUpTextBoxVisible = false;
    private void accelerationVideoButtonAction()
    {
        if (speedUpTextBoxVisible) speedUpTextBox.Hide();
        else speedUpTextBox.Show();
        speedUpTextBoxVisible = !speedUpTextBoxVisible;

    }
    private async void confirmVideoEditingButtonAction()
    {
        string arguments = string.Join(" ", ffmpegFunctions.Values);

        if(string.IsNullOrEmpty(arguments)) return;

        string path;
        if (string.IsNullOrEmpty(Path.GetExtension(fileName)) || Path.GetExtension(fileName) == "mp4")
            path = getFileName();
        else if (Path.GetExtension(fileName) != "mp4")
            path = getFileName(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
        else
            path = getFileName();

        Logger.Log($"File create in {path} with arguments {arguments}");

        initializeLoadingOverlay();
        
        await TransferFFmpegCore.Conversion(pathToFile, transferConfigManager, path, arguments);

        ClearInternal();
        
        Scheduler.AddDelayed(() => this.Push(new VideoScreen()), 2000);
    }

    private string getFileName(string name = null, string extension = null)
    {
        string fileName = (name ?? Path.GetFileNameWithoutExtension(pathToFile) + "_modified") + $"_{DateTime.Now.ToString("H/m/ss")}" +extension ?? Path.GetExtension(pathToFile);
        string path = Path.Combine(transferConfigManager.Get<string>(TransferOptions.OutputPath), fileName);
        return path;
    }
    private void initializeLoadingOverlay()
    {
        ClearInternal();
        LoadingOverlay loadingStatus;
        InternalChild = loadingStatus  = new LoadingOverlay()
        {
            Header = "Asking FFmpeg for help..."
        };
        TransferFFmpegCore.CONVERSION_STATUS.ValueChanged += (e) => loadingStatus.Header = e.NewValue;
    }
}
