using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Schema;
using osu.Framework;
using osu.Framework.Graphics.Containers;
using Transfer.Game.UserInterface.DirectoryHandler;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ExplorerFillFlowContainer : FillFlowContainer
{
    private string start_path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private string current_path;

    private TransferDirectory transferDirectory = new TransferDirectory();

    private List<ExplorerButton> explorerButtons = new List<ExplorerButton>();

    private readonly char pathSplitChar = RuntimeInfo.IsUnix ? '/' : '\\';
    private List<string> awaibleExtensions = new List<string>()
    {
        "mp4"
    };

    protected override void LoadComplete()
    {

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
            if(awaibleExtensions.Contains(Path.GetExtension(file))){
                // тут крч система должна быть, котоорая будет использовать пути файлов и показывать их.
            }
        }
    }

    public void AddExtension(string extension){
        awaibleExtensions.Add(extension);
    }

}
