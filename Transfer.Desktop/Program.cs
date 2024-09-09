using osu.Framework.Platform;
using osu.Framework;
using Transfer.Game;

namespace Transfer.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Transfer"))
            using (osu.Framework.Game game = new TransferGame())
                host.Run(game);
        }
    }
}
