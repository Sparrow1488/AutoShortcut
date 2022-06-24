namespace Sparrow.Video.Shortcuts.Exceptions
{
    public class InvalidInputPathException : Exception
    {
        public InvalidInputPathException()
        {
        }

        public InvalidInputPathException(string message) : base(message)
        {
        }

        public InvalidInputPathException(
            string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
