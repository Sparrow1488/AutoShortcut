using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class EnvironmentVariablesProvider : IEnvironmentVariablesProvider
{
    public EnvironmentVariablesProvider()
    {
        var args = System.Environment.GetCommandLineArgs();
        foreach (var argument in args)
        {
            var values = argument.Split("=");
            var key = new string(values[0].SkipWhile(x => x == '-').ToArray());
            if (values.Length > 1)
            {
                var value = values[1].Replace("\"", string.Empty);
                Variables.Add(key, value);
            }
            else Variables.Add(key, string.Empty);
        }
    }

    public Dictionary<string, string> Variables { get; } = new Dictionary<string, string>();

    public string? GetVariable(string variableName)
    {
        Variables.TryGetValue(variableName, out var value);
        return value ?? null;
    }

    public IEnvironmentVariablesProvider SetVariable(string variableName, string value)
    {
        if (Variables.ContainsKey(variableName))
            Variables[variableName] = value;
        else Variables.Add(variableName, value);
        return this;
    }
}