using System;
using System.Collections.Generic;
using FFMpegCore.Enums;
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
        SetDefault(TransferOptions.Rate, 1f);
        SetDefault(TransferOptions.AudioCodec, Codecs.libmp3lame);
        SetDefault(TransferOptions.Volume, 0.0, 0.0 , 1.0);
    }

    
}

public enum TransferOptions{
    AudioBitrate,
    Rate,
    AudioCodec,
    Volume

}

public enum Codecs
{
    libmp3lame,
    eac3,
    ac3,
    libfdk_aac,
    libvorbis,
    aac

}
