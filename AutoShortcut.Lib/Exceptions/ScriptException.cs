namespace AutoShortcut.Lib.Exceptions;

public class ScriptException : AutoShortcutException
{
    public ScriptException()
    {
    }

    public ScriptException(string? message) : base(message)
    {
    }

    public ScriptException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}