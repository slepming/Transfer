using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Graphics.Videos;

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
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        video = new TransferVideo(filename)
                        {
                            FillMode = FillMode.Fit,
                            RelativeSizeAxes = Axes.Both,
                        },

                        // mediaOptionsContainer = new Container
                        // {
                        //     Anchor = Anchor.BottomCentre,
                        //     Origin = Anchor.Centre,
                        //     Children = new Drawable[]
                        //     {
                        //         volumeContainer = new VolumeContainer
                        //         {
                        //             RelativeSizeAxes = Axes.Both
                        //         },
                        //         VolumeText = new SpriteText
                        //         {
                        //             Anchor = Anchor.BottomCentre,
                        //             Origin = Anchor.Centre,
                        //             Position = new Vector2(45, -40),
                        //             Font = new FontUsage("FiraCodeNerdFont-Light",size: 20),
                        //         },
                        //         mutedText = new SpriteText
                        //         {
                        //             Anchor = Anchor.BottomCentre,
                        //             Origin = Anchor.CentreLeft,
                        //             Position = new Vector2(-90, -40),
                        //             Text = "",
                        //             Font = new FontUsage("FiraCodeNerdFont-Light",size: 20),
                        //         },
                        //     },
                        // },
                    }
                }
            };
        }



        protected override void LoadComplete()
        {
            base.LoadComplete();
            VolumeText.Text = $"Volume {Math.Round(Audio.Volume.Value * 100, 0)}";
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
                    if(isMuted) Audio.Start();
                    else Audio.Stop();
                    isMuted = !isMuted;
                    break;
                }
                case osuTK.Input.Key.Right: Audio.Volume.Value += 5; break; // edit: нужно чтобы выходила надпись о Volume Value и пропадала
                case osuTK.Input.Key.Left: Audio.Volume.Value -= 5; break;
            }
            return base.OnKeyDown(e);
        }
    }
}
