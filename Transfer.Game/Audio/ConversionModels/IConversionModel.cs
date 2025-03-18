using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels
{
    public interface IConversionModel
    {
        /// <summary>
        /// Conversion your video with ffmpeg
        /// </summary>
        /// <param name="video">path to video</param>
        /// <param name="transferConfig">config for setting ffmpeg</param>
        /// <param name="customArguments">Custom argements for ffmpeg handling</param>
        /// <param name="conversionValue">Value for conversion</param>
        /// <returns>Path to video</returns>
        Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string customArguments = null, params object[] conversionValue);
    }
}
