using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using Transfer.Game.Graphics.Cursor;
using Transfer.Game.Screens;
using Transfer.Resources;

namespace Transfer.Game
{
    public partial class TransferGameBase : osu.Framework.Game
    {
        #if DEBUG
            public readonly string HostName = "Transfer(Developer Mode)";
        #else
            public readonly string HostName = "Transfer";
        #endif
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        protected TransferGameBase()
        {
            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                TargetDrawSize = new Vector2(1600, 900)
            });
        }

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, IRenderer renderer)
        {
            Host.Window.Title = HostName;


            Resources.AddStore(new DllResourceStore(@"Transfer.Resources.dll"));

            var fontStore = new FontStore(renderer, null, 100f);


            Fonts.AddStore(fontStore);

            Resources.Get("Fonts/");

            InitialiseFonts();

            InitialiseConfig(config);

            base.Content.Add(new TransferCursorContainer());

        }


        protected virtual void InitialiseFonts()
        {
            AddFont(Resources, @"Fonts/FiraCode/FiraCodeNerdFont");
            AddFont(Resources, @"Fonts/FiraCode/FiraCodeNerdFont-Light");
            AddFont(Resources, @"Fonts/FiraCode/FiraCodeNerdFont-Bold");

            AddFont(Resources, @"Fonts/Oswald/Oswald");
        }

        protected virtual void InitialiseConfig(FrameworkConfigManager config)
        {
            config.GetBindable<FrameSync>(FrameworkSetting.FrameSync).Value = FrameSync.VSync;
            config.GetBindable<WindowMode>(FrameworkSetting.WindowMode).Value = WindowMode.Windowed;
            config.GetBindable<RendererType>(FrameworkSetting.Renderer).Value = RendererType.Automatic;
            config.Save();
        }

        protected override bool OnExiting()
        {
            return base.OnExiting();
        }


    }
}
