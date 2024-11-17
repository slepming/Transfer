using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Game.Modules
{
    /// <summary>
    /// Loading dll dynamically
    /// </summary>
    public static class LoadLibrary
    {
        public static void LoadLibraries(string path)
        {
            if (!File.Exists(path)) return;
            LoadModule.LoadLibrary(path);
        }
    }
    public static class LoadModule
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);
    }
}
