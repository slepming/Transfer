using System;
using System.Linq;
using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels;

public class VideoModel : TransferFFmpegCore, IConversionModel
{
    [Obsolete("This function is deprecated, please use BasicModel or AudioExtractModel.")]
    public Task<string> HandleConversion(string video, TransferConfigManager transferConfig, IFileParamsBuilder fileParams = null, params string[] customArguments)
    {
        customArguments = customArguments.Append(
            $"-preset {transferConfig.Get<Presets>(TransferOptions.Preset)} -crf {transferConfig.Get<int>(TransferOptions.ConstantRateFactor)} -profile:v {transferConfig.Get<Profiles>(TransferOptions.Profile)}").ToArray();
        return Conversion(video, transferConfig, fileParams?.Build(), customArguments);
    }
}
