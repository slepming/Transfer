using System;
using System.Threading.Tasks;
using Transfer.Game.Audio.ConversionModels;
using Transfer.Game.Configuration;

namespace Transfer.Game.Audio
{
    public class ConversionBase<T> where T : IConversionModel
    {
        /// <summary>
        /// This was created to make it easier to work with the project. You can use ready-made elements or refer to the <see cref="BasicModel">basic model</see> where you have the possibility to specify your own parameters.
        /// </summary>
        /// <param name="video">path to video</param>
        /// <param name="transferConfig">config for setting ffmpeg</param>
        /// <param name="outputPath">output path(can be null, if null, then path )</param>
        /// <param name="customArguments">Custom argements for ffmpeg handling</param>
        /// <param name="conversionValue">Value for conversion</param>
        /// <returns>Path to video</returns>
        public static Task<string> Conversion(string video, TransferConfigManager transferConfig, string outputPath = null, string customArguments = null, params object[] conversionValue)
        {
            /*
             I wrote that sh*t with AI.
             Then after some time I will rewrite this module completely because of the need for proper operation and remove such crap code.
            */
            if (typeof(IConversionModel).IsAssignableFrom(typeof(T)))
            {
                var model = Activator.CreateInstance<T>();
                return model.HandleConversion(video, transferConfig, outputPath, customArguments, conversionValue);
            }

            throw new InvalidOperationException("Type T must implement IConversionModel");
        }
    }
}
