using osu.Framework.Audio.Track;

namespace Transfer.Game.Graphics.Videos;

public interface IVideoController
{
    bool IsPaused { get; }
    bool Loop { get; set; }
    double Duration { get; }
    double PlaybackPosition { get; }
    bool Pause();
    /// <summary>
    /// Changes video rate(playback speed)
    /// </summary>
    void Rate(float rate);
    double GetMaxLengthVideo();
    /// <summary>
    /// Spy seek(for seek without logging)
    /// </summary>
    void SpySeek(double time);
    /// <summary>
    /// Seek audio at the time from parameters
    /// </summary>
    void Seek(double time);
    void SetAudio(Track audio);
    /// <summary>
    /// Restarts audio at the current playback position
    /// </summary>
    bool RestartAudio();
    /// <summary>
    /// Reset video and audio position to start
    /// </summary>
    bool Reset();
}
