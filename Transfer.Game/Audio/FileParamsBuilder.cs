using Transfer.Game.Configuration;

namespace Transfer.Game.Audio;

public class FileParamsBuilder : IFileParamsBuilder
{
    private FileParams fileParams = new FileParams();

    public IFileParamsBuilder SetData(FileParams fileParams)
    {
        this.fileParams = fileParams;
        return this;
    }

    public IFileParamsBuilder SetBitrate(int bitrate)
    {
        fileParams.Bitrate = bitrate;
        return this;
    }

    public IFileParamsBuilder SetSampleRate(float sampleRate)
    {
        fileParams.Rate = sampleRate;
        return this;
    }

    public IFileParamsBuilder SetPath(string path)
    {
        fileParams.OutputPath = path;
        return this;
    }

    public IFileParamsBuilder SetAudioCodec(AudioCodecs audioCodec)
    {
        fileParams.AudioCodec = audioCodec;
        return this;
    }

    public IFileParamsBuilder SetFileName(string fileName)
    {
        fileParams.AudioFileName = fileName;
        return this;
    }

    public FileParams Build()
    {
        return fileParams;
    }
}
