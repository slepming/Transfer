using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Transfer.Game.Configuration;

internal class PlaylistConfigManager(Storage storage, IDictionary<PlaylistConfiguration, object> defaultOverrides = null) : IniConfigManager<PlaylistConfiguration>(storage, defaultOverrides)
{
    protected override string Filename => @"playlists.ini";

    protected override void InitialiseDefaults()
    {
        SetDefault(PlaylistConfiguration.Path, string.Empty);
        base.InitialiseDefaults();
    }
}

public enum PlaylistConfiguration
{
    Path,
    Name
}
