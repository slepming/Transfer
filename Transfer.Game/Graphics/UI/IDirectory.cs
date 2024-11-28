using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfer.Game.Graphics.UI
{
    public interface IDirectory
    {
        string Path { get; set; }
        string[] Files { get; }
        string[] Directories { get; }
    }
}