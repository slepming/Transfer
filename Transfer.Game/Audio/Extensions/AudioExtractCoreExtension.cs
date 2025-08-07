using System.IO;
using FFMpegCore;

namespace Transfer.Game.Audio.Extensions
{
    public static class AudioExtractCoreExtension
    {
        public static double GetAvgfps(this string file) => Path.Exists(file) ? FFProbe.Analyse(file).PrimaryVideoStream!.AverageFrameRate : throw new FileNotFoundException("File not found", file);

        public static bool ItContainsAudio(this string file) => Path.Exists(file) ? FFProbe.Analyse(file).AudioStreams.Count > 0 : throw new FileNotFoundException("File not found", file);

        public static IMediaAnalysis Analyse(this string file) => Path.Exists(file) ? FFProbe.Analyse(file) : throw new FileNotFoundException("File not found", file);
    }
}
