namespace AutoShortcut.Lib.Exceptions;

public class EffectApplyException : AutoShortcutException
{
    public EffectApplyException()
    {
    }

    public EffectApplyException(string? message) : base(message)
    {
    }

    public EffectApplyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}