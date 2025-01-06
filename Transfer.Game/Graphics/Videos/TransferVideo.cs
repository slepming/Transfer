using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Video;

namespace Transfer.Game.Graphics.Videos
{
    public partial class TransferVideo : Video
    {
        /// <summary>
        /// You can do animations when it occurs
        /// </summary>
        public Action<double> SeekOccurs;

        private bool enableRate;

        private float playbackSpeed = 1.0f;


        public TransferVideo(string filename,bool enableRate = false, bool startAtCurrentTime = true) : base(filename, startAtCurrentTime)
        {
            this.enableRate = enableRate;
        }

        [BackgroundDependencyLoader]
        private void load()
        {

        }

        protected override void Update()
        {
            if(enableRate) SpySeek(Time.Elapsed * playbackSpeed);

            base.Update();
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }

        public double GetMaxLengthVideo()
        {
            return Duration;
        }

        public void SpySeek(double time)
        {
            base.Seek(time);
        }

        public new void Seek(double time)
        {
            SeekOccurs?.Invoke(time);

            base.Seek(time);
        }

    }
}
