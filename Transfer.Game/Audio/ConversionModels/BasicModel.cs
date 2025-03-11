using System.IO;
using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels
{
    public class BasicModel : TransferFFmpegCore,IConversionModel
    {
        public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string customArguments = null, params object[] conversionValue)
        {
            string pathWithoutExtension = Path.GetFileNameWithoutExtension(video);
            return await Conversion(pathWithoutExtension, transferConfig, customArguments);
        }
    }
}
