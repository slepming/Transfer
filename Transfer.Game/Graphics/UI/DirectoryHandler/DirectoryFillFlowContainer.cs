using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public partial class DirectoryFillFlowContainer : FillFlowContainer<SpacingDirectoryText>
    {
        public string StartPath = "/";
        private string currentPath = "/";

        public Action<ValueChangedEvent<string>> FoundVideo;


        private TransferDirectory directory = new TransferDirectory();


        private List<SpacingDirectoryText> spacingDirectoryTexts = new List<SpacingDirectoryText>();

        public List<string> Extensions = new List<string>()
        {
            ".mp4"
        };


        [CanBeNull] public FontUsage Font;
        public DirectoryFillFlowContainer()
        {

        }

        private string[] findFiles(string path)
        {
            if(path == directory.Path) return findFiles(StartPath);
            currentPath = path;
            directory.Path = path;
            string[] files = directory.Directories.Union(directory.Files).ToArray();
            return files;
        }


        private void getContent(string path)
        {
            if(StartPath == null) throw new ArgumentException("Start Path is null");
            spacingDirectoryTexts.Clear();

            foreach(string file in findFiles(path ?? StartPath))
            {
                if(Extensions.Contains(Path.GetExtension(file)))
                {
                    var text = new SpacingDirectoryText(path, file)
                    {
                        Text = file.Split('/').Last(),
                        Colour = Colour4.Red,
                        Margin = new MarginPadding(15),
                        Font = new FontUsage("FiraCodeNerdFont-Bold", size: 20f),
                        FoundVideo = FoundVideo
                    };
                    spacingDirectoryTexts.Add(text);
                    text.PathChanged += onPathChange;
                }
                else
                {
                    var text = new SpacingDirectoryText(path, file)
                    {
                        Text = file.Split('/').Last(),
                        Colour = Colour4.White,
                        Margin = new MarginPadding(15),
                        Font = new FontUsage("FiraCodeNerdFont-Bold", size: 20f),
                        FoundVideo = FoundVideo
                    };
                    spacingDirectoryTexts.Add(text);
                    text.PathChanged += onPathChange;
                }
            }
            Clear();

            foreach (var text in spacingDirectoryTexts)
            {
                Add(text);
            }

        }

        protected override void LoadComplete()
        {
            getContent(StartPath);
            base.LoadComplete();
        }


        private void onPathChange(ValueChangedEvent<string> e)
        {
            getContent(e.NewValue);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if(e.Key == osuTK.Input.Key.BackSpace)
            {
                if(currentPath != StartPath)
                {
                    currentPath = System.IO.Path.GetDirectoryName(currentPath);
                    getContent(currentPath);
                }
            }
            return base.OnKeyDown(e);
        }


    }
}
