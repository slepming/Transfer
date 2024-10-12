using osu.Framework.Platform;
using osu.Framework;
using Transfer.Game;
using System.Runtime.CompilerServices;
using System;

namespace Transfer.Desktop
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string openWithTransfer = null;
            if (args.Length > 0) openWithTransfer = args[0];  
            using (GameHost host = Host.GetSuitableDesktopHost(@"Transfer"))
            using (osu.Framework.Game game = new TransferGame(openWithTransfer != null ? [openWithTransfer] : Array.Empty<string>()))
                host.Run(game);
        }
    }
}
