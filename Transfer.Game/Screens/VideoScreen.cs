#nullable disable
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Video;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using Transfer.Game.Audio;
using Transfer.Game.Graphics;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.IO;
using Transfer.Game.Screens;
using Transfer.Game.UserInterface;
using Transfer.Game.UserInterface.Containers;
using Transfer.Game.UserInterface.DirectoryHandler;
using Vulkan.Xlib;

namespace Transfer.Game.Screens
{
    public partial class VideoScreen : TransferScreen
    {

        // GameWindow window;


        // private IClock clocks; // Fix bug. Applicatio does not close
        // private readonly double delay = 1000;
        // private double lastMoveTime;

        private Loading loadingComponent;


        private Track audio;

        private VideoContainer video;

        private Box box;


        private SliderBar<double> videoPlaybackSlider;

        private ChoiceDirectory choiceDirectory;

        private SpriteText editVolumeText;

        private DrawSizePreservingFillContainer mediaOptionsContainer;
        private ScreenStack screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };

        private AudioManager audioManager;
        private Storage audioStorage;
        private ITempStore<Track> tempStore;


        private string videoPath { get; set;}
        private string audioPath { get; set; }

        public string VideoPath
        {
            get => videoPath;
            set
            {
                if (videoPath == value) return;
                videoPath = value;
            }
        }


        public VideoScreen()
        {

            // lastMoveTime = clocks.CurrentTime;
            // window = new();
        }


        [BackgroundDependencyLoader]
        private void load(AudioManager am, Storage storage)
        {
            audioManager = am;
            audioStorage = new WrappedStorage(storage.GetStorageForDirectory(@"Temp/"));
            tempStore = new TempStore<Track>(){ AudioManager = audioManager };
        }

        private void videoStartContainer()
        {

                InternalChildren = new Drawable[]
                {
                    new TooltipContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = choiceDirectory = new ChoiceDirectory
                        {
                            FoundVideo = onFoundVideo,
                        }
                    }

                };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if(e.Key == Key.LControl && e.Key == Key.R || e.Key == Key.R && e.Key == Key.LControl)
            {
                videoStartContainer();
            }
            return base.OnKeyDown(e);
        }



        protected override void LoadComplete()
        {
            base.LoadComplete();
            if (string.IsNullOrEmpty(VideoPath) || audio == null)
            {
                string nullable = audio == null ? "Audio null" : "Video null";
                Logger.Log("VideoPath or audio is null: " + nullable);
                videoStartContainer();
                return;
            }


            if (audio == null)
            {
                Logger.Log("Audio is null");
                return;
            }
            Logger.Log("Check Track success");

            InternalChildren = new Drawable[]
            {
                video = new VideoContainer(videoPath)
                {
                    Audio = audio,
                    RelativeSizeAxes = Axes.Both,
                },

            };


        }

        private async void onFoundVideo(ValueChangedEvent<string> e)
        {

            InternalChild = loadingComponent = new Loading
            {
                RelativeSizeAxes = Axes.Both
            };
            this.Push(new VideoScreen{ audio = await tempStore.GetTrackAsync(e.NewValue, audioStorage), VideoPath = e.NewValue });
        }



        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            // lastMoveTime = clocks.CurrentTime;
            // mediaOptionsContainer.Show();
            // window.CursorVisible = isDev == false ? true : false;

            return base.OnMouseMove(e);
        }

        protected override void Update()
        {

            base.Update();


        }

        protected override void OnExit()
        {

            video.Dispose();
            mediaOptionsContainer.Dispose();
            audio.Dispose();
            base.OnExit();
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }
        public override void OnSuspending(ScreenTransitionEvent e)
        {
            this.FadeIn(500, Easing.OutQuint);
        }
        public override bool OnExiting(ScreenExitEvent e)
        {
            Dispose();
            return base.OnExiting(e);
        }

        private int[] spinningBoxPosition = new int[2] { 0,0}; // -200, 20

        protected override void Dispose(bool isDisposing)
        {
            audio?.Dispose();
            video?.Dispose();
            loadingComponent?.Dispose();

            editVolumeText?.Dispose();
            mediaOptionsContainer?.Dispose();

            // if (clocks is IDisposable disposableClock)
            // {
            //     disposableClock.Dispose();
            // }
            base.Dispose(isDisposing);
        }


    }

}
