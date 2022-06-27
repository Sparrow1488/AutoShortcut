using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives
{
    public class Script : IScript
    {
        public string Command { get; set; }

        public string GetCommand()
        {
            return Command;
        }
    }
}
