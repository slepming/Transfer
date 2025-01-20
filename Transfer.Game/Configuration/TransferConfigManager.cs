using System;
using System.Collections.Generic;
using System.IO;
using osu.Framework.Configuration;
using osu.Framework.Platform;

// ReSharper disable InconsistentNaming

namespace Transfer.Game.Configuration;

public class TransferConfigManager : IniConfigManager<TransferOptions>
{
    protected override string Filename => @"app.ini";

    public TransferConfigManager(Storage storage, IDictionary<TransferOptions, object> defaultOverrides = null) : base(storage, defaultOverrides)
    {
    }

    protected override void InitialiseDefaults()
    {
        string defaultPlaylistPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos), "Standard");
        if (string.IsNullOrEmpty(defaultPlaylistPath)) defaultPlaylistPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Standard");
        SetDefault(TransferOptions.AudioBitrate, 128);
        SetDefault(TransferOptions.Rate, 1.0, 0.5, 10.0);
        SetDefault(TransferOptions.AudioCodec, AudioCodecs.aac);
        SetDefault(TransferOptions.VideoCodec, VideoCodecs.libx265);
        SetDefault(TransferOptions.Volume, 0.0, 0.0, 1.0);
        SetDefault(TransferOptions.SeekValue, 5000);
        SetDefault(TransferOptions.OutputPath, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        SetDefault(TransferOptions.Preset, Presets.faster);
        SetDefault(TransferOptions.Threads, Environment.ProcessorCount);
        SetDefault(TransferOptions.ConstantRateFactor, 23);
        SetDefault(TransferOptions.Profile, Profiles.main);
        SetDefault(TransferOptions.LoopVideo, false);
        SetDefault(TransferOptions.CurrentPlaylistPath, defaultPlaylistPath);
    }
}

public enum TransferOptions
{
    AudioBitrate,
    Rate,
    AudioCodec,
    Volume,
    SeekValue,
    OutputPath,
    VideoCodec,
    Preset,
    Threads,
    ConstantRateFactor,
    Profile,
    LoopVideo,
    CurrentPlaylistPath,
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
    libmp3lame,
    eac3,
    ac3,
    libfdk_aac,
    libvorbis,
    aac
}
public enum VideoCodecs
{
    libx264,
    libx265,
    libvpx,
    libtheora,
    png,
    mpegts
}
