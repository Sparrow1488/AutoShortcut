using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class EnvironmentVariablesProvider : IEnvironmentVariablesProvider
{
    public static EnvironmentVariableTarget EnvironmentVariableTarget => EnvironmentVariableTarget.User;

    public string GetVariable(string variableName)
        => System.Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget);

    public IEnvironmentVariablesProvider SetVariable(string variableName, string value)
    {
        System.Environment.SetEnvironmentVariable(variableName, value, EnvironmentVariableTarget);
        return this;
    }
}