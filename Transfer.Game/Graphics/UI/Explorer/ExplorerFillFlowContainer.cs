using System.Collections.Generic;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;

namespace Transfer.Game.Graphics.UI.Explorer;

public partial class ExplorerFillFlowContainer(string path) : FillFlowContainer
{
    public int SpacePosition = 5;

    [BackgroundDependencyLoader]
    private void load()
    {
        if (string.IsNullOrEmpty(path)) return;

        Direction = FillDirection.Vertical;
        loadComponents();
    }

    private void loadComponents()
    {
        var filesAndDirectories = GetFilesAndDirectoriesFromCurrentPath<ExplorerButton>(path);
        addComponentsFromPath(filesAndDirectories);
    }

    private void addComponentsFromPath(ExplorerButton[] filesAndDirectories)
    {
        foreach (var item in filesAndDirectories)
        {
            Add(item);
        }
    }

    /// <summary>
    /// Get files and directories in current explorer position
    /// </summary>
    /// <typeparam name="T">Returned object (it should be clickable)</typeparam>
    /// <returns><see cref="T"></see></returns>
    protected T[] GetFilesAndDirectoriesFromCurrentPath<T>(string currentPath) where T : ExplorerButton, new()
    {
        var files = Directory.GetFiles(currentPath);
        var directories = Directory.GetDirectories(currentPath);

        List<T> result = new List<T>();

        foreach (string directory in directories)
        {
            result.Add(new T
            {
                Text = Path.GetFileName(directory),
                IsDirectory = true,
                Current = directory
            });
        }

        foreach (string file in files)
        {
            result.Add(new T
            {
                Text = Path.GetFileNameWithoutExtension(file),
                IsDirectory = false,
                Current = file
            });
        }

        foreach (var button in result) button.Action += onSelectFile;

        return result.ToArray();
    }

    private void onSelectFile(string currentPath, bool isDirectory)
    {
        addComponentsFromPath(GetFilesAndDirectoriesFromCurrentPath<ExplorerButton>(currentPath));

        if (isDirectory)
        {
            Logger.Log("Is directory");
        }
        else
        {
            Logger.Log("Is file");
            return;
        }
    }
}
