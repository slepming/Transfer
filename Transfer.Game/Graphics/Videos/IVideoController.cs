using osu.Framework.Audio.Track;

namespace Transfer.Game.Graphics.Videos;

public interface IVideoController
{
    bool IsPaused { get; }
    bool Loop { get; set; }
    double Duration { get; }
    double PlaybackPosition { get; }
    bool Pause();
    void Rate(float rate);
    double GetMaxLengthVideo();
    void SpySeek(double time);
    void Seek(double time);
    void SetAudio(Track audio);
    bool RestartAudio();
    bool Reset();
}
