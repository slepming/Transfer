#nullable disable
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Input;
using Transfer.Game.Graphics.Cursor;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Screens
{
    public partial class VideoScreen : TransferScreen, IKeyBindingHandler<GlobalAction>
    {




        private Track audio;

        private VideoContainer video;


        private ExplorerContainer explorerContainer { get; set; } = new ExplorerContainer();

        private SpriteText editVolumeText;

        private DrawSizePreservingFillContainer mediaOptionsContainer;

        [Resolved]
        private Storage tempStorage { get; set; }


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

        public VideoScreen() {}

        public VideoScreen(Track audio, string pathToVideo)
        {
            this.audio = audio;
            VideoPath = pathToVideo;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager am)
        {
            TempStore = new TempStore<Track>(am);
            explorerContainer.FoundVideo += onFoundVideo;
            if(explorerContainer != null && !explorerContainer.IsAlive) InternalChild = (explorerContainer ??= []);
        }




        protected override void LoadComplete()
        {
            base.LoadComplete();
            if (string.IsNullOrEmpty(VideoPath))
            {
                string nullable = "Video null";
                Logger.Log(nullable);
                explorerContainer.Show();
                return;
            }

            try{
                InternalChild = new FinalContextMenuContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = video = new VideoContainer(videoPath)
                    {
                        Audio = audio,
                        RelativeSizeAxes = Axes.Both,
                    }
                };
            } catch(Exception ex)
            {
                Logger.Error(ex, "Unhandled exception: ");
                AddInternal(new ExceptionContainer{
                    HeaderText = "Error",
                    Text = ex.Message,
                    RelativeSizeAxes = Axes.Both,
                });
            }


        }

        private async void onFoundVideo(string path, bool isFile)
        {

            try
            {
                LoadingOverlay loading;
                Task<Track> trackTask = TempStore.CreateaAndGetTrackAsync(path, tempStorage);
                ClearInternal();
                AddInternal(loading = new LoadingOverlay());
                Track track = await trackTask ?? null;
                loading.Expire();

                this.Push(new VideoScreen(track, path));
            }
            catch(Exception ex){
                Logger.Error(ex, "Unhandled exception: ");
                ClearInternal();
                AddInternal(new ExceptionContainer{
                    HeaderText = "Fatal error",
                    Text = ex.Message,
                    RelativeSizeAxes = Axes.Both
                });
            }
        }

        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            if(e.Repeat)
                return false;
            Logger.Log("VideoScreen Pressed: " + e.Action.ToString());
            switch(e.Action)
            {
                case GlobalAction.OpenEditor:

                    return true;
                case GlobalAction.Explorer:
                    explorerContainer.Show();
                    return true;

            }
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {

        }
    }

}
