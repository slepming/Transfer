using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Platform;

// ReSharper disable InconsistentNaming

namespace Transfer.Game.Configuration;

public class TransferConfigManager(Storage storage, IDictionary<TransferOptions, object> defaultOverrides = null) : IniConfigManager<TransferOptions>(storage, defaultOverrides)
{
    protected override string Filename => @"app.ini";

    public IDictionary<TransferOptions, IBindable> GetCurrentConfiguration() => ConfigStore
                                                                                .Where(kvp => !CheckLookupContainsPrivateInformation(kvp.Key))
                                                                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    protected override void InitialiseDefaults()
    {
        string cachePlaylistPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Playlist");
        if (string.IsNullOrEmpty(cachePlaylistPath) || cachePlaylistPath == "Playlist") cachePlaylistPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Playlist");
        SetDefault(TransferOptions.AudioBitrate, 128);
        SetDefault(TransferOptions.Rate, 1.0, 0.5, 10.0);
        SetDefault(TransferOptions.AudioCodec, AudioCodecs.libmp3lame);
        SetDefault(TransferOptions.VideoCodec, VideoCodecs.libx265);
        SetDefault(TransferOptions.SeekValue, 5000);
        SetDefault(TransferOptions.OutputPath, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        SetDefault(TransferOptions.Preset, Presets.faster);
        SetDefault(TransferOptions.Threads, Environment.ProcessorCount);
        SetDefault(TransferOptions.ConstantRateFactor, 23);
        SetDefault(TransferOptions.Profile, Profiles.main);
        SetDefault(TransferOptions.LoopVideo, false);
        SetDefault(TransferOptions.CachePlaylistStoragePath, cachePlaylistPath);
        SetDefault(TransferOptions.CustomPlaylistStoragePath, string.Empty);
    }
}

public enum TransferOptions
{
    AudioBitrate,
    Rate,
    AudioCodec,
    SeekValue,
    OutputPath,
    VideoCodec,
    Preset,
    Threads,
    ConstantRateFactor,
    Profile,
    LoopVideo,
    CachePlaylistStoragePath,
    CustomPlaylistStoragePath,
    DarkTheme,
}

public enum Profiles
{
    baseline,
    main,
    high
}

public enum Presets
{
    ultrafast,
    superfast,
    veryfast,
    faster,
    fast,
    medium,
    slow,
    slower,
    veryslow
}

public enum AudioCodecs
{
    [Description("MP3")]
    libmp3lame,
    eac3,
    ac3,
    libfdk_aac,
    libvorbis,
    aac
}

public enum VideoCodecs
{
    [Description("X264")]
    libx264,

    [Description("X265")]
    libx265,

    libvpx,
    libtheora,
    png,
    mpegts
}
