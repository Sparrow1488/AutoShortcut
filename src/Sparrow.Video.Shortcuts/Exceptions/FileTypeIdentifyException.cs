namespace Sparrow.Video.Shortcuts.Exceptions;

public class FileTypeIdentifyException : Exception
{
    public FileTypeIdentifyException()
    {
    }

    public FileTypeIdentifyException(string message) : base(message)
    {
    }

    public FileTypeIdentifyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}