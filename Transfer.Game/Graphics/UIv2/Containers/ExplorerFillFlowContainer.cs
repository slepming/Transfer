using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Input;
using Transfer.Game.Graphics.UIv2.Buttons;

namespace Transfer.Game.Graphics.UIv2.Containers;

public partial class ExplorerFillFlowContainer : SearchContainer
{
    public Action<string> Action;

    private string currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public ExplorerFillFlowContainer()
    {
        AllowNonContiguousMatching = true;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        updateContainer(currentPath);
    }

    private bool updateContainer(string path)
    {
        if (!Directory.Exists(path)) return true;

        currentPath = path;

        IEnumerable<ExplorerButton> buttons = createRenderObjectsFromAllowedExtensions<ExplorerButton>(Directory.GetFiles(path)
                                                                                                                .Concat(Directory.GetDirectories(path)), TransferGameBase.VIDEO_EXTENSIONS.ToList())
            .OrderByDescending(button => Directory.Exists(Path.GetFullPath(button.Text.ToString())));
        Clear();

        foreach (ExplorerButton button in buttons)
        {
            button.Action += onButtonAction;
            Add(button);
        }

        return false;
    }

    private void onButtonAction(string path)
    {
        if (updateContainer(path)) Action?.Invoke(path);
    }

    private IEnumerable<T> createRenderObjectsFromAllowedExtensions<T>(IEnumerable<string> array, List<string> fileAllowedExtensions) where T : IHasText, new()
    {
        foreach (string item in array)
        {
            FileAttributes attributes = File.GetAttributes(item);
            if (!attributes.HasFlag(FileAttributes.Directory) && !fileAllowedExtensions.Contains(Path.GetExtension(item))) continue;

            yield return new T() { Text = item };
        }
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.BackSpace:
            {
                updateContainer(Directory.GetParent(currentPath)?.FullName);
                break;
            }
        }

        return base.OnKeyDown(e);
    }
}
