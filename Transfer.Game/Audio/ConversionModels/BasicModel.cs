using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels
{
    public class BasicModel : TransferFFmpegCore, IConversionModel
    {
        public string HandleConversion(string video, TransferConfigManager transferConfig, IFileParamsBuilder fileParams = null, params string[] customArguments)
        {
            return Conversion(video, transferConfig, fileParams?.Build(), customArguments: customArguments);
        }
    }
}
