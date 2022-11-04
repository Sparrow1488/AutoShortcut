namespace Sparrow.Video.Abstractions.Services;

public interface IEnvironmentVariablesProvider
{
    string GetVariable(string variableName);
    IEnvironmentVariablesProvider SetVariable(string variableName, string value);
}
