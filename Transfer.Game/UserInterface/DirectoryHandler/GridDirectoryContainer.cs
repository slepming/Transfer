using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    [Obsolete("Use DirectoryScrollContainer",true)]
    public partial class GridDirectoryContainer(string path = @"/home/slepming") : GridContainer
    {
        private string latestPath;
        private string currentPath = path;
        private string nextPath = "";
        private TransferDirectory transferDirectory = new TransferDirectory();
        private List<SpacingDirectoryText> spacingTexts = new List<SpacingDirectoryText>();
        private FontUsage fontUsage = new FontUsage(size: 50);
        private string[] files = new string[]{};


        private string[] FindFiles(string newPath)
        {
            if(currentPath == null) Logger.Error(new NullReferenceException(), "Path is null");
            if(transferDirectory.Path == newPath) return files;
            transferDirectory.Path = newPath;
            string[] strings = transferDirectory.Directories.Union(transferDirectory.Files).ToArray();
            files = strings;
            return strings;
        }

        private void JoinDrawable(string newPath)
        {
            spacingTexts.Clear();
            string[] allFiles = FindFiles(newPath);
            int column = 3 + CalculateColumn(allFiles.Length);
            foreach(string file in FindFiles(newPath))
            {
                string[] fileName = file.Split('/');
                var text = new SpacingDirectoryText(currentPath, currentPath + file)
                {
                    Text = fileName[fileName.Length-1] ?? "",
                    Font = fontUsage,
                };

                text.PathChanged += onPathChanged; 
                spacingTexts.Add(text);

            }

            Drawable[] drawables = spacingTexts.Select(text => (Drawable)text).ToArray();

            var gridContent = new Drawable[drawables.Length / 3 + (drawables.Length % 3 > 0 ? 1 : 0) + (drawables.Length % 6 > 0 ? 2 : 0) + (drawables.Length >= 16 ? 4 : 1) + (drawables.Length >= 40 ? 128 : 0)][];
            
            for (int i = 0; i < gridContent.Length; i++)
            {
                gridContent[i] = drawables.Skip(i * column).Take(column).ToArray();
            }
            Content = gridContent;
        }
        [BackgroundDependencyLoader]
        private void load()
        {
            JoinDrawable(path);
        }

        protected override void LoadComplete()
        {

            base.LoadComplete();
        }

        private void onPathChanged(ValueChangedEvent<string> e)
        {
            Logger.Log("Changed path");
            latestPath = currentPath;
            JoinDrawable(e.NewValue);
        }

        private int CalculateColumn(int amount)
        {
            switch(amount)
            {
                case <6: return -1;
                case int n when n > 6 && n < 12: return 1;
                case int n when n > 12 && n < 24: fontUsage = new FontUsage(size: 35); return 2;
                case int n when n > 24: fontUsage = new FontUsage(size: 20); return 6;
                case > 50: fontUsage = new FontUsage(size: 8); return 8;
                default: return 0;
            }
        }
    }
}