namespace Transfer.Game.Graphics.UI
{
    public interface IDirectory
    {
        string Path { get; set; }
        string[] Files { get; }
        string[] Directories { get; }
    }
}