namespace AutoShortcut.Lib.Exceptions;

public class UnknownFileException : AutoShortcutException
{
    public UnknownFileException()
    {
    }

    public UnknownFileException(string? message) : base(message)
    {
    }

    public UnknownFileException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}