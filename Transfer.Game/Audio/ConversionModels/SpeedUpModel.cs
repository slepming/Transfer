using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;

namespace Transfer.Game.Audio.ConversionModels;

public class SpeedUpModel : TransferFFmpegCore, IConversionModel
{
    public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string outputPath = null, string customArguments = null, params object[] conversionValue)
    {
        string extension = Path.GetExtension(video);
        string pathWithoutExtension = Path.GetFileNameWithoutExtension(video);
        outputPath ??= Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(pathWithoutExtension.ToLower())}");
        CONVERSION_STATUS.Value = "Check conversion value";
        float audioFactor = 1;
        if (conversionValue[0] != null) audioFactor = (float)conversionValue[0];
        float videoFactor = 1 / audioFactor;
        string arguments = customArguments + FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoSpeedUp, videoFactor.ToString(CultureInfo.InvariantCulture)) + FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.AudioSpeedUp, audioFactor.ToString(CultureInfo.InvariantCulture));
        return await Conversion(pathWithoutExtension, transferConfig, outputPath, arguments, extension);
    }
}
