using JetBrains.Annotations;
using Transfer.Game.Graphics.Videos;

namespace Transfer.Game.Graphics.UI;

public interface ICanUpdateVideo
{
    void UpdateVideo(string path);
    void UpdateVideo([NotNull] TransferVideo video = null);
}
