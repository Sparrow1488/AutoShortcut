namespace AutoShortcut.Lib.Exceptions;

public class HandleException : AutoShortcutException
{
    public HandleException()
    {
    }

    public HandleException(string? message) : base(message)
    {
    }

    public HandleException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}