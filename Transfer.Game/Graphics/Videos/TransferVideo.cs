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
using osu.Framework.Graphics.Video;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osuTK;
using Transfer.Game.UserInterface;
using Transfer.Game.UserInterface.Containers;

namespace Transfer.Game.Graphics.Videos
{
    public partial class TransferVideo : Video, IHasContextMenu
    {
        public MenuItem[] ContextMenuItems => new MenuItem[]{
            new MenuItem("Convert to")
        };

        public TransferVideo(string filename, bool startAtCurrentTime = true) : base(filename, startAtCurrentTime)
        {

        }

        [BackgroundDependencyLoader]
        private void load()
        {

        }


        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }



    }
}
