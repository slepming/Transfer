namespace Transfer.Game.Testing
{
    /// <summary>
    /// Designed to easily manipulate data in Transfer.Tests without conflicts directly from dependencies.
    /// </summary>
    public class VideoTestingByte(byte[] data)
    {
        public byte[] Data { get; } = data;
    }
}
