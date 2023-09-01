namespace AutoShortcut.Lib.Exceptions;

public class ConfigurationException : AutoShortcutException
{
    public ConfigurationException()
    {
    }

    public ConfigurationException(string? message) : base(message)
    {
    }

    public ConfigurationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}