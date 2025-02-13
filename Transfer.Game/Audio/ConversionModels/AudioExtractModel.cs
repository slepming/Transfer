using System;
using System.IO;
using System.Threading.Tasks;
using osu.Framework.Logging;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;

namespace Transfer.Game.Audio.ConversionModels
{
    public sealed class AudioExtractModel : TransferFFmpegCore, IConversionModel
    {
        public async Task<string> HandleConversion(string video, TransferConfigManager transferConfig, string outputPath = null, string customArguments = null, params object[] conversionValue)
        {
            if (string.IsNullOrEmpty(video))
            {
                throw new ArgumentNullException(nameof(video));
            }

            string pathWithoutExtension = Path.GetFileNameWithoutExtension(video);
            outputPath ??= Path.Combine(Path.GetTempPath(), $"{Hash.GetHashString(pathWithoutExtension.ToLower())}.mp3");
            return await Conversion(video, transferConfig, outputPath, customArguments, "mp3");
        }
    }
}
