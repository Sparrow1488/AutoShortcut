namespace Sparrow.Video.Abstractions.Exceptions;

public class InputResolutionNameNotRequiredException : Exception
{
    public InputResolutionNameNotRequiredException()
    {
    }

    public InputResolutionNameNotRequiredException(string? message) : base(message)
    {
    }
}
