namespace Sparrow.Video.Shortcuts.Exceptions;

public class IgnoreFileUploadedException : Exception
{
    public IgnoreFileUploadedException()
    {
    }

    public IgnoreFileUploadedException(string message) : base(message)
    {
    }
}