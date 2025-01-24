using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Logging;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerFillFlowContainer : SearchContainer
{
    private string start_path => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private string current_path;

    private TransferDirectory transferDirectory = new TransferDirectory();

    private List<ExplorerButton> explorerButtons = new List<ExplorerButton>();



    /// <summary>
    /// If file extension not in List then it's skipping
    /// </summary>
    private List<string> awaibleExtensions = TransferGameBase.VIDEO_EXTENSIONS.ToList();

    /// <summary>
    /// Fired when pressing the button
    /// </summary>
    public event TransitionEvent TransitionPath;

    public Bindable<string> PathChanged => new Bindable<string>();


    public ExplorerFillFlowContainer()
    {
        AllowNonContiguousMatching = true;

    }



    protected override void LoadComplete()
    {
        getContent(start_path);
        base.LoadComplete();
    }
    private string[] files(string path)
    {
        if(path == transferDirectory.Path) return files(start_path);
        current_path = path;
        transferDirectory.Path = current_path;
        PathChanged.Value = path;
        return transferDirectory.Directories.Union(transferDirectory.Files).ToArray();
    }

    private void getContent(string path)
    {
        explorerButtons.Clear();
        foreach(string file in files(path ?? start_path))
        {
            ExplorerButton explorerButton;
            if(awaibleExtensions.Contains(Path.GetExtension(file).ToLower())){
                explorerButton = new()
                {
                    Path = file,
                    TextColour = Colour4.Blue
                };
            }
            else
            {
                explorerButton = new()
                {
                    Path = file
                };
            }
            explorerButtons.Add(explorerButton);
            explorerButton.Transition += changeDirectoryExplorer;
        }
        Clear();
        foreach (ExplorerButton button in explorerButtons) Add(button);
        Add(new Box
        {
            Colour = Colour4.Transparent,
            RelativeSizeAxes = Axes.X,
            Height = 50
        });
    }

    private void changeDirectoryExplorer(string path, bool isFile)
    {
        try
        {
            string fullPath = Path.GetFullPath(path);

            if (File.Exists(fullPath))
            {
                if (awaibleExtensions.Contains(Path.GetExtension(fullPath)))
                {
                    TransitionPath?.Invoke(fullPath, true);
                    return;
                }
                else
                {
                    Logger.Log($"The file was found, but its extension is invalid: {fullPath}. Extension: {System.IO.Path.GetExtension(fullPath)}");
                    return;
                }
            }
            if (Directory.Exists(fullPath))
            {
                getContent(fullPath);
                TransitionPath?.Invoke(path, false);
                return;
            }
        }
        catch (DirectoryNotFoundException)
        {
            TransitionPath?.Invoke(path, true);
        }
    }

    public void ExplorerPathChange(string path)
    {
        if(path != current_path)
        {
            getContent(path);
        }
    }

    public void AddExtension(string extension) => awaibleExtensions.Add(extension);

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == osuTK.Input.Key.BackSpace)
        {
            if (current_path != start_path)
            {
                ExplorerPathChange(Path.GetDirectoryName(current_path));
            }
        }
        return base.OnKeyDown(e);
    }
}
