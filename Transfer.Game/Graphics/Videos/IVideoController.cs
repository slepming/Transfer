using osu.Framework.Audio.Track;

namespace Transfer.Game.Graphics.Videos;

public interface IVideoController
{
    void Pause();
    void Rate(float rate);
    double GetMaxLengthVideo();
    void SpySeek(double time);
    void Seek(double time);
    void SetAudio(Track audio);
}
