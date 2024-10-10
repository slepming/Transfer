using System;

namespace Transfer.Game.Extensions;

[System.Serializable]
public class TransformException : System.Exception
{
    public TransformException() { }
    public TransformException(string message) : base(message) { }
    public TransformException(string message, System.Exception inner) : base(message, inner) { }
}
