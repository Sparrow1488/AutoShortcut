using Sparrow.Video.Abstractions.Builders;
using Sparrow.Video.Abstractions.Builders.Formats;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Primitives;
using System.Text;

namespace Sparrow.Video.Shortcuts.Builders;

public class ScriptBuilder : IScriptBuilder
{
    public ScriptBuilder()
    {
        _stringBuilder = new StringBuilder();
        _firstArgumentsList = new List<string>();
        _lastArgumentsList = new List<string>();
    }

    private readonly StringBuilder _stringBuilder;
    private readonly List<string> _firstArgumentsList;
    private readonly List<string> _lastArgumentsList;

    public IScriptBuilder Insert(string argument)
    {
        _firstArgumentsList.Add(argument);
        return this;
    }

    public IScriptBuilder InsertLast(string argument)
    {
        _lastArgumentsList.Add(argument);
        return this;
    }

    public string BuildCommand()
    {
        var joined = _firstArgumentsList.Concat(_lastArgumentsList);
        _stringBuilder.AppendJoin(' ', joined);
        return _stringBuilder.ToString();
    }

    public IScript BuildScript()
    {
        var script = new Script()
        {
            Command = BuildCommand()
        };
        return script;
    }

    public IScript BuildScript(IScriptBuilderFormat format)
    {
        var formattedInputCommands = format.UseFormat(_firstArgumentsList);
        _firstArgumentsList.Clear();
        _firstArgumentsList.AddRange(formattedInputCommands);
        return BuildScript();
    }
}