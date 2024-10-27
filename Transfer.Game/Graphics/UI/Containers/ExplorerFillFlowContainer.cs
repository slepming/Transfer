using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Schema;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK.Graphics;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerFillFlowContainer : FillFlowContainer
{
    private string start_path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private string current_path;

    private TransferDirectory transferDirectory = new TransferDirectory();

    private List<ExplorerButton> explorerButtons = new List<ExplorerButton>();


    private List<string> awaibleExtensions = TransferGameBase.VIDEO_EXTENSIONS.ToList();

    public event TransitionEvent TransitionPath;

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
    }

    private void changeDirectoryExplorer(string path, bool isFile)
    {
        try
        {
            string fullPath = System.IO.Path.GetFullPath(path);

            if (System.IO.File.Exists(fullPath))
            {
                if (awaibleExtensions.Contains(System.IO.Path.GetExtension(fullPath)))
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
            if (System.IO.Directory.Exists(fullPath))
            {
                getContent(fullPath);
                return;
            }
        }
        catch (DirectoryNotFoundException)
        {
            TransitionPath?.Invoke(path, true);
        }
    }

    public void AddExtension(string extension){
        awaibleExtensions.Add(extension);
    }
    protected override bool OnKeyDown(KeyDownEvent e)
    {
        if (e.Key == osuTK.Input.Key.BackSpace)
        {
            if (current_path != start_path)
            {
                current_path = System.IO.Path.GetDirectoryName(current_path);
                getContent(current_path);
            }
        }
        return base.OnKeyDown(e);
    }
}
