using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Containers.Menu;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Graphics.UI.Containers.Overlays;

public partial class WatchingToolsContainer : TransferFocusedOverlayContainer
{
    public VolumeContainer VolumeContainer;
    public VideoPlaybackContainer PlaybackContainer;

    private readonly SpriteText currentTimecodeText;

    public Color4 BackgroundColour
    {
        set => background.Colour = value;
    }

    private Box background;
    private Container videoBox;
    private Container toolsBox;

    private SpriteButton backButton, nextButton, nextSeekButton, backSeekButton, stopButton;
    private SpriteButton repeatButton, settingsButton;

    private ExtensionMenu extensionMenu = new ExtensionMenu();

    [Resolved]
    private TransferConfigManager configManager { get; set; }

    [NotNull]
    public VideoContainer Video;

    private string timecode = string.Empty;

    /// <summary>
    /// This seek space is needed to indicate the gap between transitions from one point in the video timeline to another, as requested by the user. This functionality already exists in VideoContainer; they are the same thing. I just don't want to rewrite the hotkeys for this container
    /// </summary>
    private int seekSpace;

    public WatchingToolsContainer() => Show();

    [BackgroundDependencyLoader]
    private void load()
    {
        seekSpace = configManager.Get<int>(TransferOptions.SeekValue);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        InternalChildren =
        [
            background = new Box
            {
                Colour = Colour4.FromHex("#010216"),
                RelativeSizeAxes = Axes.Both,
                Depth = 3
            },
            videoBox = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = Video,
                Depth = 2,
            },
            toolsBox = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(Size.X, Size.Y / 10),
                Depth = 1,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Masking = true,
                BorderColour = Colour4.Gray.Opacity(80),
                BorderThickness = 3,
                Children =
                [
                    new Box // background
                    {
                        Colour = Colour4.Black,
                        Alpha = 0.5f,
                        RelativeSizeAxes = Axes.Both
                    },
                    PlaybackContainer = new VideoPlaybackContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(Size.X, 3),
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                    },
                    new TransferContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Children =
                        [
                            settingsButton = new SpriteButton
                            {
                                TextureName = "settings",
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Margin = new MarginPadding(5),
                                Size = new Vector2(25, 25),
                            },
                            new TransferContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 3, Size.Y),
                                Children =
                                [
                                    nextButton = new SpriteButton
                                    {
                                        TextureName = "next",
                                        Anchor = Anchor.CentreRight,
                                        Origin = Anchor.Centre,
                                        Margin = new MarginPadding(50),
                                        Size = new Vector2(50, 50)
                                    },
                                    backButton = new SpriteButton
                                    {
                                        TextureName = "back",
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.Centre,
                                        Margin = new MarginPadding(50),
                                        Size = new Vector2(50, 50)
                                    },
                                    nextSeekButton = new SpriteButton
                                    {
                                        TextureName = "nextSeek",
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.CentreLeft,
                                        // Position = new Vector2(50, 0),
                                        Margin = new MarginPadding(50),
                                        Size = new Vector2(50, 50)
                                    },
                                    backSeekButton = new SpriteButton
                                    {
                                        TextureName = "backSeek",
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.CentreRight,
                                        // Position = new Vector2(-50, 0),
                                        Margin = new MarginPadding(50),
                                        Size = new Vector2(50, 50)
                                    },
                                    stopButton = new SpriteButton
                                    {
                                        TextureName = "play",
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Size = new Vector2(50, 50)
                                    }
                                ],
                            },
                            repeatButton = new SpriteButton
                            {
                                TextureName = "repeatBlack",
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                Margin = new MarginPadding(5),
                                Size = new Vector2(50, 50),
                            },
                        ]
                    }
                ]
            }
        ];

        changeTextureRepeatButton(configManager.Get<bool>(TransferOptions.LoopVideo));
        nextButton.Action += onNextVideoClick;
        nextSeekButton.Action += positiveSeek;
        stopButton.Action += onClickPlayButton;
        backButton.Action += onBackVideoClick;
        backSeekButton.Action += negativeSeek;

        settingsButton.Action += onOpenSettings;
        repeatButton.Action += onActivateRepeat;

        SetMaxPlaybackValue(Video.GetDuration());
    }

    private void negativeSeek()
    {
        Video.Seek(-seekSpace);
    }

    private void positiveSeek()
    {
        Video.Seek(seekSpace);
    }

    private void onBackVideoClick()
    {
        throw new System.NotImplementedException();
    }

    private void onNextVideoClick()
    {
        throw new System.NotImplementedException();
    }

    private void onOpenSettings()
    {
        if (extensionMenu.Visible) extensionMenu.Hide();
        else extensionMenu.Show();
    }

    private void onActivateRepeat()
    {
        bool videoLoopFromConfig = configManager.Get<bool>(TransferOptions.LoopVideo);
        configManager.SetValue(TransferOptions.LoopVideo, !videoLoopFromConfig);
        Video.SetVideoLoop(videoLoopFromConfig);
        changeTextureRepeatButton(videoLoopFromConfig);
    }

    private void changeTextureRepeatButton(bool value) => repeatButton.TextureName = value ? "repeatWhite" : "repeatBlack";

    private void onClickPlayButton()
    {
        throw new System.NotImplementedException();
    }

    private long duration;

    public void SetMaxPlaybackValue(double value)
    {
        PlaybackContainer.SetMaxSliderValue(value);
        duration = (long)value;
    }

    protected override void PopIn()
    {
    }

    protected override void PopOut()
    {
    }
}
