using osu.Framework.Platform;

namespace Transfer.Game.IO;

public partial class TempStorage : WrappedStorage
{
    public TempStorage(Storage underlyingStorage, string subPath = null)
        : base(underlyingStorage, subPath)
    {
    }
}
