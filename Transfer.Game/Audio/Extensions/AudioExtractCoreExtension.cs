using FFMpegCore;

namespace Transfer.Game.Audio.Extensions
{
    public static class AudioExtractCoreExtension
    {
        public static double GetAVGFPS(string file) => FFProbe.Analyse(file).PrimaryVideoStream.AvgFrameRate;
    }
}
