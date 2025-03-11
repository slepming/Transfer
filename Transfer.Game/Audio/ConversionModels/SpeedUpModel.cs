using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels;

public class SpeedUpModel : TransferFFmpegCore, IConversionModel
{
    public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string customArguments = null, params object[] conversionValue)
    {
        string extension = Path.GetExtension(video);
        string pathWithoutExtension = Path.GetFileNameWithoutExtension(video);
        CONVERSION_STATUS.Value = "Check conversion value";
        float audioFactor = 1;
        if (conversionValue[0] != null) audioFactor = (float)conversionValue[0];
        float videoFactor = 1 / audioFactor;
        string arguments = customArguments + FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.VideoSpeedUp, videoFactor.ToString(CultureInfo.InvariantCulture)) + FFmpegArgument.GetVideoEditingArgument(VideoEditingFunction.AudioSpeedUp, audioFactor.ToString(CultureInfo.InvariantCulture));
        return await Conversion(pathWithoutExtension, transferConfig, arguments, extension);
    }
}
