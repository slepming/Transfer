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
            using (GameHost host = Host.GetSuitableDesktopHost(@"Transfer"))
            using (osu.Framework.Game game = new TransferGame(args))
                host.Run(game);
        }
    }
}
