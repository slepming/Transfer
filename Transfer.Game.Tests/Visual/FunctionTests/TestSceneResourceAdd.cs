using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Transfer.Game.Audio;

namespace Transfer.Game.Tests.Visual.FunctionTests
{
    public partial class TestSceneResourceAdd : TransferTestScene
    {
        private AudioExtractorCore extractAudio = new AudioExtractorCore();
        private string testVideoPath = @"/home/slepming/Загрузки/Youtube/MerionAcademy/Все о принципах SOLID.mp4";
        [BackgroundDependencyLoader]
        private async void load(Storage storage, AudioManager audioManager)
        {
            Storage audioStorage = new TestStorage(storage.GetStorageForDirectory(@"Temp/"));
            string audioPath = await extractAudio.Extract(testVideoPath);
            using(var audioFile = audioStorage.GetStream(audioPath.Split('/').Last(), FileAccess.Write, FileMode.Create))
                using(FileStream readAudio = new FileStream(audioPath, FileMode.Open, FileAccess.Read))
                {
                    await readAudio.CopyToAsync(audioFile);
                }
            IResourceStore<byte[]> resourceStore = new StorageBackedResourceStore(audioStorage);
            Track track = audioManager.GetTrackStore(resourceStore).Get(audioPath.Split('/').Last());
            if(track == null) Logger.Error(new Exception(), "Track is null");
            await track.StartAsync();
            track.Volume.Value = 1;
            Logger.Log("Trak isn't null " + track.Name);
            await Task.Delay(20000);
            await track.StopAsync();

        }

        private class TestStorage : Storage
        {
            protected Storage UnderlyingStorage { get; private set; }

            private readonly string subPath;

            public TestStorage(Storage underlyingStorage, string subPath = null)
                : base(string.Empty)
            {
                ChangeTargetStorage(underlyingStorage);

                this.subPath = subPath;
            }

            protected virtual string MutatePath(string path)
            {
                if (path == null)
                    return null;

                return !string.IsNullOrEmpty(subPath) ? Path.Combine(subPath, path) : path;
            }

            protected virtual void ChangeTargetStorage(Storage newStorage)
            {
                UnderlyingStorage = newStorage;
            }

            public override string GetFullPath(string path, bool createIfNotExisting = false) =>
                UnderlyingStorage.GetFullPath(MutatePath(path), createIfNotExisting);

            public override bool Exists(string path) =>
                UnderlyingStorage.Exists(MutatePath(path));

            public override bool ExistsDirectory(string path) =>
                UnderlyingStorage.ExistsDirectory(MutatePath(path));

            public override void DeleteDirectory(string path) =>
                UnderlyingStorage.DeleteDirectory(MutatePath(path));

            public override void Delete(string path) =>
                UnderlyingStorage.Delete(MutatePath(path));

            public override IEnumerable<string> GetDirectories(string path) =>
                ToLocalRelative(UnderlyingStorage.GetDirectories(MutatePath(path)));

            public IEnumerable<string> ToLocalRelative(IEnumerable<string> paths)
            {
                string localRoot = GetFullPath(string.Empty);

                foreach (string path in paths)
                    yield return Path.GetRelativePath(localRoot, UnderlyingStorage.GetFullPath(path));
            }

            public override IEnumerable<string> GetFiles(string path, string pattern = "*") =>
                ToLocalRelative(UnderlyingStorage.GetFiles(MutatePath(path), pattern));

            public override Stream GetStream(string path, FileAccess access = FileAccess.Read, FileMode mode = FileMode.OpenOrCreate) =>
                UnderlyingStorage.GetStream(MutatePath(path), access, mode);

            public override void Move(string from, string to) => UnderlyingStorage.Move(MutatePath(from), MutatePath(to));

            public override bool OpenFileExternally(string filename) => UnderlyingStorage.OpenFileExternally(MutatePath(filename));

            public override bool PresentFileExternally(string filename) => UnderlyingStorage.PresentFileExternally(MutatePath(filename));

            public override Storage GetStorageForDirectory(string path)
            {
                ArgumentException.ThrowIfNullOrEmpty(path);

                if (!path.EndsWith(Path.DirectorySeparatorChar))
                    path += Path.DirectorySeparatorChar;

                // create non-existing path.
                GetFullPath(path, true);

                return new TestStorage(this, path);
            }
        }
    }


}
