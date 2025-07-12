using System;
using osu.Framework;
using osu.Framework.Platform;

namespace Transfer.Game.Tests;

public static class VisualTestRunner
{
    [STAThread]
    public static int Main(string[] args)
    {
        using (DesktopGameHost host = Host.GetSuitableDesktopHost(@"Transfer-development"))
        {
            host.Run(new TransferTestBrowser());
            return 0;
        }
    }
}
