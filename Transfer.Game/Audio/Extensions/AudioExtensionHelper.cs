using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfer.Game.Audio.Extensions
{
    public static class AudioExtensionHelper
    {
        private static readonly Dictionary<AudioExtension, string> extension_strings = new Dictionary<AudioExtension, string>
        {
            { AudioExtension.mp3, "mp3" },
            { AudioExtension.aac, "aac" }
        };

        public static string GetExtensionString(AudioExtension extension)
        {
            if (extension_strings.TryGetValue(extension, out var extensionString))
            {
                return extensionString;
            }
            throw new ArgumentException("Unsupported audio extension");
        }
    }
}
