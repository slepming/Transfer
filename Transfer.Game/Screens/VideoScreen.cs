#nullable disable
using System;
using System.ComponentModel;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Input;
using Transfer.Game.Graphics.Cursor;
using Transfer.Game.IO;
using Transfer.Game.UserInterface.Containers;
using Transfer.Game.UserInterface.DirectoryHandler;

namespace Transfer.Game.Screens
{
    public partial class VideoScreen : TransferScreen
    {

        // GameWindow window;


        // private IClock clocks; // ! It's pretty weird how it works. The app closes
        // private readonly double delay = 1000;
        // private double lastMoveTime;



        private Track audio;

        private VideoContainer video;




        private ChoiceDirectory choiceDirectory;

        private SpriteText editVolumeText;

        private DrawSizePreservingFillContainer mediaOptionsContainer;

        [Resolved]
        private Storage audioTempStorage { get; set; }

        protected ITempStore<Track> TempStore { get; set; }


        private string videoPath { get; set;}

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

        public VideoScreen(string arg)
        {
            VideoPath = arg;
        }
        public VideoScreen(Track audio, string[] args)
        {
            this.audio = audio;
            VideoPath = args[0];
        }
        public VideoScreen(Track audio, string pathToVideo)
        {
            this.audio = audio;
            VideoPath = pathToVideo;
        }


        [BackgroundDependencyLoader]
        private void load(AudioManager am)
        {
            TempStore = new TempStore<Track>(){ AudioManager = am };
            AddInternal(choiceDirectory = new ChoiceDirectory
            {
                FoundVideo = onFoundVideo,
            });
            choiceDirectory.Hide();
        }

        private void videoStartContainer()
        {

            choiceDirectory.Show();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if(e.Key == Key.LControl && e.Key == Key.R || e.Key == Key.R && e.Key == Key.LControl)
            {
                videoStartContainer();
            }
            return base.OnKeyDown(e);
        }



        protected override async void LoadComplete()
        {
            base.LoadComplete();
            if (string.IsNullOrEmpty(VideoPath))
            {
                string nullable = "Video null";
                Logger.Log(nullable);
                videoStartContainer();
                return;
            }


            if (audio == null)
            {
                this.Push(new VideoScreen(await TempStore.GetTrackAsync(videoPath, audioTempStorage), videoPath));
                return;
            }
            Logger.Log("Check Track success");

            try{
                InternalChildren = new Drawable[]
                {
                    new FinalContextMenuContainer{
                        RelativeSizeAxes = Axes.Both,
                        Child = video = new VideoContainer(videoPath)
                        {
                            Audio = audio,
                            RelativeSizeAxes = Axes.Both,
                        },
                    }

                };
            } catch(Exception ex)
            {
                AddInternal(new ExceptionContainer{
                    HeaderText = "Error",
                    Text = ex.Message,
                    RelativeSizeAxes = Axes.Both,
                });
            }


        }

        private async void onFoundVideo(ValueChangedEvent<string> e)
        {

            try
            {
                this.Push(new VideoScreen(await TempStore.GetTrackAsync(e.NewValue, audioTempStorage), e.NewValue));
            }
            catch(Exception ex){
                ClearInternal();
                AddInternal(new ExceptionContainer{
                    HeaderText = "FFmpeg Error",
                    Text = ex.Message,
                    RelativeSizeAxes = Axes.Both
                });
            }
        }



        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            // lastMoveTime = clocks.CurrentTime;
            // mediaOptionsContainer.Show();
            // window.CursorVisible = isDev == false ? true : false;

            return base.OnMouseMove(e);
        }
        public override bool OnExiting(ScreenExitEvent e)
        {
            Dispose();
            return base.OnExiting(e);
        }


        protected override void Dispose(bool isDisposing)
        {
            audio?.Dispose();
            video?.Dispose();
            LoadingComponent?.Dispose();

            editVolumeText?.Dispose();
            mediaOptionsContainer?.Dispose();

            base.Dispose(isDisposing);
        }


    }

}
