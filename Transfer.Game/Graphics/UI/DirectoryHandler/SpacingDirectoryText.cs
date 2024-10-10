using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FFmpeg.NET.Services;
using OpenTabletDriver.Plugin.Platform.Pointer;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Input.StateChanges;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using Transfer.Game.Screens;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public partial class SpacingDirectoryText(string path,string newpath) : SpriteText
    {
        /// <summary>
        /// File path which you will use(path to file)
        /// </summary>
        private string pathToFile = newpath;

        /// <summary>
        /// Current path whitch you use
        /// </summary>
        private string currentPath = path;


        public List<string> Extensions = new List<string>()
        {
            ".mp4"
        };


        public Action<ValueChangedEvent<string>> PathChanged;
        public Action<ValueChangedEvent<string>> FoundVideo;

        protected override bool OnClick(ClickEvent e)
        {
            if(Extensions.Contains(Path.GetExtension(pathToFile)))
            {
                if(File.Exists(pathToFile))
                {
                    FoundVideo?.Invoke(new ValueChangedEvent<string>(currentPath, pathToFile));
                }
                else
                {
                    Logger.Error(new FileNotFoundException(), "File not found");
                }
            }
            else{
                PathChanged?.Invoke(new ValueChangedEvent<string>(currentPath, pathToFile));
                Logger.Log("Step by directory");
            }



            return base.OnClick(e);
        }
        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if(e.Key == osuTK.Input.Key.Right)
                this.FadeColour(Colour4.SkyBlue, 500, Easing.InOutQuad);
            return base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyUpEvent e)
        {
            if(e.Key == osuTK.Input.Key.Right)
                this.FadeColour(Colour, 500, Easing.InOutQuad);

            base.OnKeyUp(e);
        }


        protected override void LoadComplete()
        {
            Margin = new MarginPadding { Horizontal = 5, Vertical = 5 };
            base.LoadComplete();
        }
        protected override bool OnHover(HoverEvent e)
        {
            this.TransformSpacingTo(new Vector2(0.5f,0),2000, Easing.OutElastic);
            return base.OnHover(e);
        }
        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.TransformSpacingTo(new Vector2(0,0),2000, Easing.OutElastic);
            base.OnHoverLost(e);
        }


    }
}
