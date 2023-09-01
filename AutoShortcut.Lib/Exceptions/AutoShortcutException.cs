namespace AutoShortcut.Lib.Exceptions;

public abstract class AutoShortcutException : Exception
{
    protected AutoShortcutException()
    {
    }

    protected AutoShortcutException(string? message) : base(message)
    {
    }

    protected AutoShortcutException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}