namespace AutoCursorLock.App;

using System;

public class AutoCursorLockException : Exception
{
    public AutoCursorLockException(string message) : base(message)
    {
    }
}
