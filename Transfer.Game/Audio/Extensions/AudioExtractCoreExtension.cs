using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFMpegCore;

namespace Transfer.Game.Audio.Extensions
{
    public class AudioExtractCoreExtension
    {
        public static double GetAVGFPS(string file) => FFProbe.Analyse(file).PrimaryVideoStream.AvgFrameRate;
    }
}
