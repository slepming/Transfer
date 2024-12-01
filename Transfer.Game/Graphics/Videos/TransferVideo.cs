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

        public TransferVideo(string filename, bool startAtCurrentTime = true) : base(filename, startAtCurrentTime)
        {

        }

        [BackgroundDependencyLoader]
        private void load()
        {

        }

        protected override void Update()
        {
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

        public new void Seek(double time)
        {
            base.Seek(time);
            SeekOccurs?.Invoke(time);
        }

    }
}
