using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using Transfer.Game.Graphics.Cursor;
using Transfer.Game.Graphics.Videos;
using Transfer.Game.Screens;

namespace Transfer.Game.UserInterface.Containers
{
    public partial class VideoContainer : Container
    {
        private TransferVideo video;

        private VolumeContainer volumeContainer;
        private Container mediaOptionsContainer;
        

        public SpriteText VolumeText;
        private SpriteText mutedText;

        public Track Audio;

        private bool isMuted = false;

        private string filename;


        public VideoContainer(string filename)
        {
            this.filename = filename;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if(string.IsNullOrWhiteSpace(filename)) Logger.Log("File path is null or file don't find");
            Logger.Log("Video initialization");


            InternalChildren = new Drawable[]
            {
                video = new TransferVideo(filename)
                {
                    FillMode = FillMode.Fit,
                    RelativeSizeAxes = Axes.Both,
                    Loop = false,
                },
                volumeContainer = new VolumeContainer
                {
                    Width = 300,
                    Height = 50,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                },
            };
            
        }




        protected override void LoadComplete()
        {
            if(video.IsFaulted){
                AddInternal(new ExceptionContainer{
                    HeaderText = "Error",
                    Text = "Decoder error",
                    RelativeSizeAxes = Axes.Both,
                });
            }
            base.LoadComplete();
        }

        protected override void LoadAsyncComplete()
        {
            base.LoadAsyncComplete();
            Audio.StartAsync();
        }


        private void onVolumeChanged(ValueChangedEvent<double> e)
        {
            Audio.Volume.Value = (float)e.NewValue;

            VolumeText.Text = $"Volume {Math.Round(e.NewValue*100,0)}";

            if(e.NewValue == 0)
                mutedText.Text = "Muted!";
            else
                mutedText.Text = "";
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch(e.Key)
            {
                case osuTK.Input.Key.Space:
                {
                    if(isMuted)
                    {
                        Audio.Start();
                        video.IsPlaying = true;
                    }
                    else
                    {
                        Audio.Stop();
                        video.IsPlaying = false;
                    }
                    isMuted = !isMuted;
                    
                    break;
                }
                case osuTK.Input.Key.Right: Audio.Volume.Value += 5; break;
                case osuTK.Input.Key.Left: Audio.Volume.Value -= 5; break;
            }
            return base.OnKeyDown(e);
        }
        protected override bool OnScroll(ScrollEvent e)
        {
            Audio.Volume.Value += e.ScrollDelta.Y / 10;
            return base.OnScroll(e);
        }
    }
}
