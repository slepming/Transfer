using FFMpegCore;

namespace Transfer.Game.Audio.Extensions
{
    public static class AudioExtractCoreExtension
    {
        public static double GetAVGFPS(string file) => FFProbe.Analyse(file).PrimaryVideoStream!.AvgFrameRate;
        public static bool ItContainsAudio(string file) => FFProbe.Analyse(file).AudioStreams.Count > 0;

        public static IMediaAnalysis Analyse(string file) => FFProbe.Analyse(file);
    }
}
