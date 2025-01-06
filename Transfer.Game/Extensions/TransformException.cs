namespace Transfer.Game.Extensions;

[System.Serializable]
public class TransferException : System.Exception
{
    public TransferException() { }
    public TransferException(string message) : base(message) { }
    public TransferException(string message, System.Exception inner) : base(message, inner) { }
}
