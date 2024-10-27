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


        [Resolved]
        private ExplorerContainer explorerContainer { get; set; }
        [Resolved]
        private ConfigurationContainer configurationContainer { get; set; }

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
            TempStore = new TempStore<Track>();
            explorerContainer.FoundVideo += onFoundVideo;
            if(!explorerContainer.IsAlive) InternalChild = explorerContainer;
            //if(!configurationContainer.IsAlive) InternalChild = configurationContainer;
        }




        protected override async void LoadComplete()
        {
            base.LoadComplete();
            if (string.IsNullOrEmpty(VideoPath))
            {
                string nullable = "Video null";
                Logger.Log(nullable);
                explorerContainer.Show();
                return;
            }


            if (audio == null)
            {
                this.Push(new VideoScreen(await TempStore.CreateaAndGetTrackAsync(videoPath, tempStorage), videoPath));
                return;
            }
            Logger.Log("Check Track success");

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
                this.Push(new VideoScreen(await TempStore.CreateaAndGetTrackAsync(path, tempStorage), path));
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

        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            if(e.Repeat)
                return false;
            Logger.Log(e.Action.ToString());
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
