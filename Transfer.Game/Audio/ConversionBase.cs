using System.Threading.Tasks;
using Transfer.Game.Audio.ConversionModels;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio;

public static class ConversionBase
{
    /// <summary>
    /// This was created to make it easier to work with the project. You can use ready-made elements or refer to the <see cref="BasicModel">basic model</see> where you have the possibility to specify your own parameters.
    /// </summary>
    /// <param name="video">path to video</param>
    /// <param name="transferConfig">config for setting ffmpeg</param>
    /// <param name="fileParams"></param>
    /// <param name="customArguments">Custom argements for ffmpeg handling</param>
    /// <returns>Path to video</returns>
    public static Task<string> Conversion<T>(string video, TransferConfigManager transferConfig, IFileParamsBuilder fileParams = null, params string[] customArguments) where T : IConversionModel, new()
    {
        IConversionModel model = new T();
        return model.HandleConversion(video, transferConfig, fileParams, customArguments: customArguments);
    }
}
