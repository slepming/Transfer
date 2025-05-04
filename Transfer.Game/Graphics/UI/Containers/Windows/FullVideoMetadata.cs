using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using Transfer.Game.Audio.Extensions;

namespace Transfer.Game.Graphics.UI.Containers.Windows;

public partial class FullVideoMetadata([NotNull] string path) : MenuWindow
{
    private string videoPath = path ?? string.Empty;

    [BackgroundDependencyLoader]
    private void load()
    {
        var metadata = AudioExtractCoreExtension.Analyse(videoPath);
        WindowContent.Add(new TextFlowContainer
        {
            Text = $"{metadata.Format}\n"
                   + $"{metadata.Duration}\n"
                   + $"{string.Join('\n', metadata.ErrorData)}"
        });
    }
}
