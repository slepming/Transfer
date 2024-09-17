#nullable disable
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

        private AudioManager audioManager;
        private Storage audioStorage;
        private ITempStore<Track> tempStore;


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


            this.Push(new VideoScreen{ audio = await tempStore.GetTrackAsync(e.NewValue, audioStorage), VideoPath = e.NewValue });
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
