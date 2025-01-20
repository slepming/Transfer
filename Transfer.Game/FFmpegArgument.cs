using System.Collections.Generic;

namespace Transfer.Game;
public static class FFmpegArgument
{
    private static readonly Dictionary<VideoEditingFunction, string> video_editing_arguments = new Dictionary<VideoEditingFunction, string>()
    {
        { VideoEditingFunction.VideoSpeedUp, "-vf setpts={0}*PTS"},
        { VideoEditingFunction.AudioSpeedUp, "-af atempo={0}"},
    };
    public static string GetVideoEditingArgument(VideoEditingFunction function, params object[] args)
    {
        if(video_editing_arguments.TryGetValue(function, out var videoArgument))
        {
            return string.Format(videoArgument, args);
        }
        return null;
    }


}

public enum VideoEditingFunction{
    VideoSpeedUp,
    AudioSpeedUp,
}