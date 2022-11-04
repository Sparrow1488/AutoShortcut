using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Enums;

namespace Sparrow.Video.Shortcuts.Extensions;

public static class EnvironmentVariablesProviderExtensions
{
    public static string CurrentProjectOpenMode(this IEnvironmentVariablesProvider environment)
        => environment.GetVariable(EnvironmentVariableNames.ProjectOpenMode);

    public static bool IsSerialize(this IEnvironmentVariablesProvider environment)
        => bool.Parse(environment.GetVariable(EnvironmentVariableNames.Serialize));

    public static string OutputFileName(this IEnvironmentVariablesProvider environment)
        => environment.GetVariable(EnvironmentVariableNames.OutputName);

    public static bool IsDevelopment(this IEnvironmentVariablesProvider environment)
    {
        var variable = environment.GetVariable(EnvironmentVariableNames.Environment);
        return variable == "development" || variable == "dev";
    }
}
