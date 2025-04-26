using System;
using Transfer.Game.Configuration;

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
            if (audioFileName == value) return;

            audioFileName = value;
        }
    }

    public string FileExtension { get; set; }

    public override string ToString()
    {
        return $"Rate: {Rate}, Bitrate: {Bitrate}, OutputPath: {OutputPath}, AudioCodec: {AudioCodec}";
    }
}
