namespace Sparrow.Video.Shortcuts.Exceptions
{
    public class EmptyOrNullArgumentException : Exception
    {
        public EmptyOrNullArgumentException()
        {
        }

        public EmptyOrNullArgumentException(string message) : base(message)
        {
        }
    }
}
