using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfer.Game.UserInterface.DirectoryHandler
{
    public interface IDirectory
    {
        string Path { get; set; }
        string[] Files { get; }
        string[] Directories { get; }
    }
}