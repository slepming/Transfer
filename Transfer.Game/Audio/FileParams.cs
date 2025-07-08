using System;
using Transfer.Game.Configuration;
using Path = System.IO.Path;

namespace Transfer.Game.Audio;

public class FileParams
{
    /// <summary>
    /// Audio and video speed
    /// </summary>
    public float Rate { get; set; } = 1;

    /// <summary>
    /// Audio bitrate
    /// </summary>
    public int Bitrate { get; set; } = 64;

    /// <summary>
    /// Output path for file(only directory)
    /// </summary>
    public string OutputPath { get; set; }

    /// <summary>
    /// Codec for ffmpeg audio
    /// </summary>
    public AudioCodecs AudioCodec { get; set; }

    private string audioFileName;

    public string AudioFileName
    {
        get
        {
            if (FileExtension == null) throw new ArgumentNullException(nameof(FileExtension) + " equal null");

            return audioFileName + FileExtension;
        }
        set
        {
            if (audioFileName != null && audioFileName == value) return;

            audioFileName = Path.GetFileNameWithoutExtension(value);
        }
    }

    public string FileExtension => initializeExtension(AudioCodec);

    private string initializeExtension(AudioCodecs audioCodec)
    {
        switch (audioCodec)
        {
            case AudioCodecs.aac: return @".aac";

            case AudioCodecs.libmp3lame: return @".mp3";

            default: return @".aac";
        }
    }

    public override string ToString()
    {
        return $"Rate: {Rate}, Bitrate: {Bitrate}, OutputPath: {OutputPath}, AudioCodec: {AudioCodec}, File name: {audioFileName}, File extension: {FileExtension}";
    }
}
