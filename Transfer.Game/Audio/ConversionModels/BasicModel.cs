using System.IO;
using System.Threading.Tasks;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;

namespace Transfer.Game.Audio.ConversionModels
{
    public class BasicModel : TransferFFmpegCore,IConversionModel
    {
        public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string outputPath = null, string customArguments = null, params object[] conversionValue)
        {
            string pathWithoutExtension = Path.GetFileNameWithoutExtension(video);
            outputPath ??= Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(pathWithoutExtension.ToLower())}");
            return await Conversion(pathWithoutExtension, transferConfig, outputPath, customArguments, Path.GetExtension(outputPath));
        }

    }
}
