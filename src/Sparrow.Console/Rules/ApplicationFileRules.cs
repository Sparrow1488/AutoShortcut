namespace Sparrow.Console.Rules
{
    public class ApplicationFileRules
    {
        public static readonly LoopFileRule LoopFileRule = new(file => true)
        {
            LoopCount = 2
        };
        public static readonly FormatFileRule FormatFileRule = new(file => true);
        public static readonly SilentFileRule SilentFileRule = new(file => true);
    }
}
