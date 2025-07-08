using JetBrains.Annotations;
using Transfer.Game.Graphics.Videos;

namespace Transfer.Game.Graphics.UI;

public interface ICanUpdateVideo
{
    void UpdateVideo(string path, [NotNull] TransferVideo video = null);
}
