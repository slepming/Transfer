using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Transfer.Game.Configuration;

internal class PlaylistConfigManager(Storage storage, IDictionary<PlaylistConfiguration, object> defaultOverrides = null) : IniConfigManager<PlaylistConfiguration>(storage, defaultOverrides)
{
    protected override string Filename => @"playlists.ini";

    protected override void InitialiseDefaults()
    {
        // На самом деле у меня сомнения на счет этого способа хранения состояний плейлистов,
        // лучше уж было создать JSON файлы там,
        // но я вероятнее всего буду фиксировать последнее состояние плейлиста(то есть его путь, если он есть),
        // Если его нет, то путь останется так и нулевым.
        // Было бы хорошо хранить все плейлисты внутри хранилища приложения, так доступ будет легче, но пожалуй дам выбор пользователю.
        SetDefault(PlaylistConfiguration.Path, string.Empty);
        base.InitialiseDefaults();
    }
}

public enum PlaylistConfiguration
{
    Path,
    Name
}
