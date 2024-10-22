using System;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.IO;
using Transfer.Game.Screens;

namespace Transfer.Game
{
    public partial class TransferGame : TransferGameBase
    {
        private ScreenStack screenStack;
        private Screen transferScreen;

        private string openWithTransferArgument;

        [Cached]
        private readonly ExplorerContainer explorerContainer = new ExplorerContainer();

        [Cached]
        private readonly ConfigurationContainer configurationContainer = new ConfigurationContainer() { RelativeSizeAxes = Axes.Both } ;

        public TransferGame(string[] args)
        {
            if(args.Length > 0 ) openWithTransferArgument = args[0];
        }
        public TransferGame() { }

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
        }

        protected override void LoadAsyncComplete()
        {
            base.LoadAsyncComplete();
            if(openWithTransferArgument != null) screenStack.Push(transferScreen = new VideoScreen(openWithTransferArgument));
            else screenStack.Push(transferScreen = new VideoScreen());

        }

    }
}
