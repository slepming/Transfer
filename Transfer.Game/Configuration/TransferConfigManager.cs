using System;
using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Transfer.Game.Configuration;

public class TransferConfigManager : IniConfigManager<TransferOptions>
{
    protected override string Filename => @"app.ini";
    public TransferConfigManager(Storage storage, IDictionary<TransferOptions, object> defaultOverrides = null) : base(storage, defaultOverrides)
    {
    }
    protected override void InitialiseDefaults()
    {
        SetDefault(TransferOptions.AudioBitrate, 128);
        SetDefault(TransferOptions.Rate, 1.0, 0.5, 10.0);
        SetDefault(TransferOptions.AudioCodec, AudioCodecs.libmp3lame);
        SetDefault(TransferOptions.VideoCodec, VideoCodecs.libx265);
        SetDefault(TransferOptions.Volume, 0.0, 0.0, 1.0);
        SetDefault(TransferOptions.SeekValue, 5000);
        SetDefault(TransferOptions.OutputPath, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

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
    VideoCodec

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
