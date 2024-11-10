using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Threading;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Input.Bindings;
using Transfer.Game.IO;
using Transfer.Game.Screens;

namespace Transfer.Game
{
    public partial class TransferGame : TransferGameBase, IKeyBindingHandler<GlobalAction>, ICanAcceptFile
    {
        private Screen transferScreen;

        private readonly string[] args;


        [Resolved]
        private Storage tempStorage { get; set; }


        private ITempStore<Track> tempStore;
        private DependencyContainer dependencies;

        public TransferGame(string[] args)
        {
            this.args = args;
        }
        public TransferGame() { }

        [BackgroundDependencyLoader]
        private void load()
        {
            tempStore = new TempStore<Track>();
            
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            if(dependencies.TryGet(out AudioManager audioManager))
            {
                if(dependencies.TryGet(out ScreenStack screenStack))
                {
                    if (args != null && args.Length > 0)
                    {
                        List<string> allowedPaths = new List<string>();
                        foreach (string path in args)
                        {
                            string fileName = Path.GetFileName(path);
                            if (tempStorage.Exists($"{fileName}"))
                            {
                                if (VIDEO_EXTENSIONS.Contains(Path.GetExtension(path)))
                                {
                                    allowedPaths.Add(path);
                                }
                            }
                        }
                        lock (allowedPaths)
                        {
                            Import(allowedPaths.ToArray());
                        }
                        allowedPaths.Clear();
                        Scheduler.AddDelayed(async () =>
                        {
                            Track audio = await tempStore.CreateaAndGetTrackAsync(Path.GetFileName(args[0]).Split('.')[0] + ".mp3", tempStorage, audioManager);
                            if (audio != null) screenStack.Push(transferScreen = new VideoScreen(audio, pathToVideo: args[0]));
                        }, 200, true);
                    }
                    else
                    {
                        Logger.Log("Audio File not exist");
                        Scheduler.AddDelayed(() =>
                        {
                            screenStack.Push(transferScreen = new VideoScreen());
                        }, 500);
                    }
                }
            }
        }

        private readonly List<string> dropFiles = new List<string>();
        private ScheduledDelegate dropScheduledDelegate;

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            if(host.Window != null)
            {
                host.Window.DragDrop += path =>
                {
                    if (VIDEO_EXTENSIONS.Contains(Path.GetExtension(path)))
                    {
                        lock (dropFiles)
                        {
                            dropFiles.Add(path);
                            Logger.Log(@$"File ""{System.IO.Path.GetFileName(path)}"" been importing");

                            dropScheduledDelegate?.Cancel();
                            dropScheduledDelegate = Scheduler.AddDelayed(handleImportFromDrop, 100);
                        }
                    }
                    else
                    {
                        Logger.Log("Unhandled extension");
                    }
                };
            }
        }

        private void handleImportFromDrop()
        {
            lock (dropFiles)
            {
                Logger.Log($"Handling of {dropFiles.Count} files");
                string[] paths = dropFiles.ToArray();
                dropFiles.Clear();
                Task.Factory.StartNew(() => Import(dropFiles.ToArray()), TaskCreationOptions.LongRunning);
            }
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));



        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            Logger.Log($"TransferGame Pressed: {e.Action.ToString()}");
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {

        }

        public Task Import(params string[] paths)
        {
            lock (paths)
            {
                if (paths.Length == 0) return Task.CompletedTask;
                if (paths.Length == 1)
                {
                    tempStore.CreateTrackInStorageAsync(System.IO.Path.GetFullPath(paths[0]), tempStorage);
                }
                foreach(string path in paths)
                {
                    if (path == null) continue;
                    if (!File.Exists(Path.GetFullPath(path)))
                    {
                        Logger.Log($"File {Path.GetFileName(Path.GetFullPath(path))} is not exists");
                        continue;
                    }
                    tempStore.CreateTrackInStorageAsync(Path.GetFullPath(path), tempStorage);
                }
                return Task.CompletedTask;
            }
        }
    }
}
