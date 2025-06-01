using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels
{
    public class BasicModel : TransferFFmpegCore, IConversionModel
    {
        public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, IFileParamsBuilder fileParams = null, params string[] customArguments)
        {
            return await Conversion(video, transferConfig, fileParams?.Build(), customArguments: customArguments);
        }
    }
}
