using System;
using System.IO;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels
{
    public sealed class AudioExtractModel : TransferFFmpegCore, IConversionModel
    {
        public string HandleConversion(string video, TransferConfigManager transferConfig, IFileParamsBuilder fileParams = null, params string[] customArugments)
        {
            if (string.IsNullOrEmpty(video))
                throw new ArgumentNullException(nameof(video));
            _ = Path.GetFileNameWithoutExtension(video);
            return Conversion(video, transferConfig, fileParams?.Build(), customArguments: customArugments);
        }
    }
}
