using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Logging;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public class TransferDirectory : IDirectory
    {
        public TransferDirectory(string path)
        {
            Path = path;
        }
        public TransferDirectory()
        {

        }
        public string Path
        {
            get => path;

            set
            {
                if(path == value) return;
                if(Directory.Exists(value)) path = value;
                else Logger.Error(new DirectoryNotFoundException(), $"Directory path {value} not found");
            }

        }
        private string path;
        public int CountFiles { get => (Files.Length + Directories.Length); }
        public string[] Files => files;

        private string[] files => Directory.GetFiles(Path,"*.mp4");

        public string[] Directories => directories;
        private string[] directories => Directory.GetDirectories(Path);

    }
}