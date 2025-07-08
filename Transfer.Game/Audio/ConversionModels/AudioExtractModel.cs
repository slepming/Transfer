using System;
using System.IO;
using System.Threading.Tasks;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio.ConversionModels
{
    public sealed class AudioExtractModel : TransferFFmpegCore, IConversionModel
    {
        public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, IFileParamsBuilder fileParams = null, params string[] customArugments)
        {
            if (string.IsNullOrEmpty(video))
                throw new ArgumentNullException(nameof(video));

            string pathWithoutExtension = Path.GetFileNameWithoutExtension(video);
            return await Conversion(video, transferConfig, fileParams?.Build(), customArguments: customArugments);
        }
    }
}
