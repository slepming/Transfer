using System.IO;
using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels;

public class VideoModel : TransferFFmpegCore, IConversionModel
{
    public Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string customArguments = null, params object[] conversionValue)
    {
        string extension = Path.GetExtension(video);
        customArguments += $"-preset {transferConfig.Get<Presets>(TransferOptions.Preset)} -crf {transferConfig.Get<int>(TransferOptions.ConstantRateFactor)} -profile:v {transferConfig.Get<Profiles>(TransferOptions.Profile)}";
        return Conversion(video, transferConfig, customArguments, extension);
    }
}
