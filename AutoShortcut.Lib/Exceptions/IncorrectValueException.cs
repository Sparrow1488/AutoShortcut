namespace AutoShortcut.Lib.Exceptions;

public class IncorrectValueException : AutoShortcutException
{
    public IncorrectValueException()
    {
    }

    public IncorrectValueException(string? message) : base(message)
    {
    }

    public IncorrectValueException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}