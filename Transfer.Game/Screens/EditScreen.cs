using System;
using osu.Framework.Allocation;
using Transfer.Game.Graphics.UI;
using osu.Framework.Graphics;
using Transfer.Game.Graphics.UI.Containers;
using osuTK;
using Transfer.Game.IO;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.Platform;
using Transfer.Game.Configuration;
using System.IO;
using osu.Framework.Logging;
using osu.Framework.Graphics.Shapes;
using System.Collections.Generic;
using Transfer.Game.Audio;
using osu.Framework.Bindables;
using osu.Framework.Screens;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using Transfer.Game.Extensions;
using Transfer.Game.Audio.ConversionModels;
using Transfer.Game.Graphics.UI.Containers.Dialogs;
using Transfer.Game.Graphics.UI.Containers.Overlays;
using Transfer.Game.Graphics.Videos;

namespace Transfer.Game.Screens;

public partial class EditScreen : TransferScreen, ICanUpdateVideo
{
    private readonly string pathToFile = string.Empty;
    private string fileName = string.Empty;
    private SpriteText timeCode;
    private readonly Dictionary<VideoEditingFunction, string> ffmpegFunctions = new();
    private StringButton confirmButton;

    private EditToolsContainer editToolsContainer;

    private VideoContainer videoContainer;

    private AudioExtract<Track> audioExtract;

    private FileSelectorDialog explorerContainer;

    private Container toolsContainer, videoWithBackgroundContainer;

    private const float window_scaling_factor_x = 1.05f;

    [Resolved]
    private Storage audioTempStorage { get; set; }

    [Resolved]
    private TransferConfigManager transferConfigManager{ get; set; }

    public EditScreen() { }
    public EditScreen(VideoContainer video) => videoContainer = video;
    public EditScreen(string pathToFile) => this.pathToFile = pathToFile;

    [BackgroundDependencyLoader]
    private void load(AudioManager audioManager)
    {
        audioExtract = new AudioExtract<Track>(transferConfigManager);
        explorerContainer = new(this);

        if (videoContainer == null)
        {
            if (pathToFile == null)
            {
                explorerContainer.Show();
            }

            videoContainer = new VideoContainer(pathToFile)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Horizontal = 10 },
                Depth = 1,
            };
        }

        videoWithBackgroundContainer = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Position = new Vector2(0, -40),
            Size = new Vector2(Size.X / window_scaling_factor_x, Size.Y / 1.2f),
            Masking = true,
            CornerRadius = 10,
            Children =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.FromHex("#0a0a2e"),
                    Depth = 5
                },
                videoContainer,
                timeCode = new SpriteText
                {
                    Text = "",
                    Font = new FontUsage(TransferFonts.FiraCodeNerdFont),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Depth = 0,
                }
            ]
        };

        toolsContainer = new Container
        {
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(Size.X / window_scaling_factor_x, Size.Y / 12),
            Position = new Vector2(0, -50),
            Masking = true,
            CornerRadius = 10,
            Children =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 1,
                    Colour = Colour4.FromHex("#052266"),
                    Depth = 5,
                },
                editToolsContainer = new EditToolsContainer
                {
                    Depth = 2,
                }
            ]
        };
        editToolsContainer.SpeedUpValue.ValueChanged += onSpeedVideoChange;
        editToolsContainer.ConfirmAction += confirmVideoEditingButtonAction;

        confirmButton = new StringButton
        {
            Text = "Confirm",
            Width = 100,
            Height = 50,
            Anchor = Anchor.BottomCentre,
            Origin = Anchor.BottomCentre,
        };
        confirmButton.Action += confirmVideoEditingButtonAction;

        Box background = new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.FromHex("#030625"),
            Alpha = 0.3f,
            Depth = 0
        };

        Scheduler.AddDelayed(() =>
        {
            InternalChildren =
            [
                background,
                toolsContainer,
                videoWithBackgroundContainer
            ];
        }, 500);
    }

    private void onSpeedVideoChange(ValueChangedEvent<double> e)
    {
        videoContainer.Rate((float)e.NewValue);
        commitAccelerationTextBox(e.NewValue);
    }

    private void onChangeValueInNameTextBox(ValueChangedEvent<string> e)
    {
        fileName = e.NewValue;
    }

    private void commitAccelerationTextBox(double sender)
    {
        float factor = (float)sender;
    }

    protected override void Update()
    {
        base.Update();
        if (timeCode != null && videoContainer != null) timeCode.Text = videoContainer.GetTimecode();
    }

    protected override void OnNotifyError(string text)
    {
        this.FlashColour(Colour4.Red, 1000);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
    }

    private async void confirmVideoEditingButtonAction()
    {
        string arguments = string.Join(" ", ffmpegFunctions.Values) ?? null;

        if (string.IsNullOrEmpty(arguments)) return;

        string path;
        if (string.IsNullOrEmpty(Path.GetExtension(fileName)) || Path.GetExtension(fileName) == "mp4")
            path = getFileName(Path.GetFileNameWithoutExtension(fileName), "mp4");
        else if (Path.GetExtension(fileName) != "mp4")
            path = getFileName(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
        else
            path = getFileName(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));

        Logger.Log($"File create in {path} with arguments {arguments}");

        initializeLoadingOverlay();

        await ConversionBase.Conversion<BasicModel>(pathToFile, transferConfigManager, new FileParamsBuilder().SetPath(path), arguments); // Don't forget set Bitrate, Codec from config

        ClearInternal();

        Scheduler.AddDelayed(() => this.Push(new VideoScreen()), 2000);
    }

    private string getFileName(string name, string extension)
    {
        string fileName = name + "_modified" + $"_{DateTime.Now.ToString("H/m/ss")}" + Path.GetExtension(pathToFile);
        string path = Path.Combine(transferConfigManager.Get<string>(TransferOptions.OutputPath), fileName);
        return path;
    }

    private void initializeLoadingOverlay()
    {
        ClearInternal();
        InternalChild = new LoadingOverlay()
        {
            Header = "Asking FFmpeg for help..."
        };
    }

    public override bool OnExiting(ScreenExitEvent e)
    {
        videoContainer.Expire();
        return base.OnExiting(e);
    }

    public void UpdateVideo(string path)
    {
        videoContainer.UpdateVideo(path);
    }

    public void UpdateVideo(TransferVideo video = null)
    {
        throw new NotImplementedException();
    }
}
