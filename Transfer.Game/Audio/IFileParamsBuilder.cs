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

    IFileParamsBuilder SetFileName(string fileName);

    FileParams Build();
}
