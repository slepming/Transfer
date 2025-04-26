using Transfer.Game.Configuration;

namespace Transfer.Game.Audio;

public interface IFileParamsBuilder
{
    IFileParamsBuilder SetData(FileParams fileParams);
    IFileParamsBuilder SetBitrate(int bitrate);
    IFileParamsBuilder SetSampleRate(float sampleRate);

    /// <summary>
    /// Path without file name
    /// </summary>
    /// <param name="path">Path to output file</param>
    /// <returns></returns>
    IFileParamsBuilder SetPath(string path);

    IFileParamsBuilder SetAudioCodec(AudioCodecs audioCodec);

    /// <summary>
    /// Function hashed you name and return in ffmpeg
    /// </summary>
    /// <param name="fileName">Not hash file name</param>
    /// <returns></returns>
    IFileParamsBuilder SetFileName(string fileName);

    IFileParamsBuilder SetExtension(string extension);

    FileParams Build();
}
