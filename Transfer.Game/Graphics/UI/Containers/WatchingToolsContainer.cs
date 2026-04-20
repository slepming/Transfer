using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Graphics.UI.Buttons;
using Transfer.Game.Graphics.UIv2.Containers;
using Transfer.Game.Graphics.Videos;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class WatchingToolsContainer : Container
{
    public VolumeContainer VolumeContainer;
    public VideoPlaybackContainer PlaybackContainer;

    private Container toolsBox;

    private DynamicToolButton negativeSpeed, doubleSpeed, nextSeekButton, backSeekButton, stopButton;
    private ToolsButton repeatButton, settingsButton;

    [Resolved]
    private TransferConfigManager configManager { get; set; }

    [CanBeNull]
    private IVideoController videoController;

    /// <summary>
    /// This seek space is needed to indicate the gap between transitions from one point in the video timeline to another, as requested by the user. This functionality already exists in VideoContainer; they are the same thing. I just don't want to rewrite the hotkeys for this container
    /// </summary>
    private int seekSpace;

    public WatchingToolsContainer()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;
        Anchor = Anchor.BottomCentre;
        Origin = Anchor.BottomCentre;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        seekSpace = configManager.Get<int>(TransferOptions.SeekValue);
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        if (videoController is null) throw new Exception("Video is null");

        InternalChildren =
        [
            toolsBox = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Masking = true,
                Children =
                [
                    new Box
                    {
                        Colour = Colour4.FromHSL(0, 0, 0.09f),
                        Alpha = 0.7f,
                        RelativeSizeAxes = Axes.Both
                    },
                    PlaybackContainer = new VideoPlaybackContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(Size.X, 5),
                        Duration = videoController.Duration,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre
                    },
                    new TransferContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Padding = new MarginPadding(25),
                        Child =
                            new FillFlowContainer
                            {
                                AutoSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Direction = FillDirection.Horizontal,
                                Spacing = new Vector2(435),
                                Children =
                                [
                                    //settingsButton = new ToolsButton(FontAwesome.Solid.Spinner)
                                    //{
                                    //    Size = new Vector2(25, 25)
                                    //},
                                    new FillFlowContainer
                                    {
                                        AutoSizeAxes = Axes.Both,
                                        Direction = FillDirection.Horizontal,
                                        Spacing = new Vector2(50),
                                        Children =
                                        [
                                            negativeSpeed = new DynamicToolButton()
                                            {
                                                Size = new Vector2(50, 50),
                                                Icons = [FontAwesome.Solid.LongArrowAltLeft]
                                            },
                                            backSeekButton = new DynamicToolButton()
                                            {
                                                Size = new Vector2(50, 50),
                                                Icons = [FontAwesome.Solid.ArrowLeft]
                                            },
                                            stopButton = new DynamicToolButton()
                                            {
                                                Size = new Vector2(50, 50),
                                                Icons = [FontAwesome.Solid.Stop, FontAwesome.Solid.Play]
                                            },
                                            nextSeekButton = new DynamicToolButton()
                                            {
                                                Size = new Vector2(50, 50),
                                                Icons = [FontAwesome.Solid.ArrowRight]
                                            },
                                            doubleSpeed = new DynamicToolButton()
                                            {
                                                Size = new Vector2(50, 50),
                                                Icons = [FontAwesome.Solid.LongArrowAltRight]
                                            }
                                        ]
                                    },
                                    //repeatButton = new ToolsButton(FontAwesome.Regular.Circle)
                                    //{
                                    //    Text = "Repeat",
                                    //    Size = new Vector2(50, 50)
                                    //}
                                ]
                            }
                    }
                ]
            }
        ];

        doubleSpeed.Action += onDoubleSpeedClick;
        nextSeekButton.Action += positiveSeek;
        stopButton.Action += onClickPlayButton;
        negativeSpeed.Action += onNegativeSpeedClick;
        backSeekButton.Action += negativeSeek;

        // settingsButton.Action += onOpenSettings;
        //repeatButton.Action += onActivateRepeat;
        PlaybackContainer.SliderBar.UserCommit.ValueChanged += flexSeek;

        Scheduler.AddDelayed(() =>
        {
            if (PlaybackContainer.SliderBar.Current.Value != videoController.PlaybackPosition)
            {
                PlaybackContainer.SetSliderBarValue(videoController.PlaybackPosition);
            }
        }, 1000, true);
    }

    /// <summary>
    /// Update playback slider duration value and set VideoController
    /// </summary>
    /// <param name="videoController">Video Controller</param>
    public void SetVideoController(IVideoController videoController)
    {
        this.videoController = videoController;
        if (PlaybackContainer != null)
            PlaybackContainer.Duration = videoController.Duration;
    }

    private void negativeSeek() =>
        videoController?.Seek(-seekSpace);

    private void positiveSeek() =>
        videoController?.Seek(seekSpace);

    private void onNegativeSpeedClick() =>
        videoController?.Rate(0.5f);

    private void onDoubleSpeedClick() =>
        videoController?.Rate(2);

    private void flexSeek(ValueChangedEvent<double> seek) =>
        videoController?.Seek(seek.NewValue);

    private void onActivateRepeat()
    {
        bool videoLoopFromConfig = configManager.Get<bool>(TransferOptions.LoopVideo);
        configManager.SetValue(TransferOptions.LoopVideo, !videoLoopFromConfig);
        if (videoController != null) videoController.Loop = videoLoopFromConfig;
    }

    private void onClickPlayButton()
    {
        videoController?.Pause();
    }
}
