using Sparrow.Video.Abstractions.Builders;
using System.Text;

namespace Sparrow.Video.Shortcuts.Builders
{
    public class CommandBuilder : ICommandBuilder
    {
        public CommandBuilder()
        {
            _stringBuilder = new StringBuilder();
            _firstArgumentsList = new List<string>();
            _lastArgumentsList = new List<string>();
        }

        private readonly StringBuilder _stringBuilder;
        private readonly IList<string> _firstArgumentsList;
        private readonly IList<string> _lastArgumentsList;

        public ICommandBuilder Insert(string argument)
        {
            _firstArgumentsList.Add(argument);
            return this;
        }

        public ICommandBuilder InsertLast(string argument)
        {
            _lastArgumentsList.Add(argument);
            return this;
        }

        public string Build()
        {
            var joined = _firstArgumentsList.Concat(_lastArgumentsList);
            _stringBuilder.AppendJoin(' ', joined);
            return _stringBuilder.ToString();
        }
    }
}
