using JetBrains.Annotations;
using Transfer.Game.Graphics.Videos;

namespace Transfer.Game.Graphics.UI;

/// <summary>
/// Interface for object that can Update video
/// </summary>
public interface ICanUpdateVideo
{
    void UpdateVideo(string path);
    void UpdateVideo([NotNull] TransferVideo video = null);
}
