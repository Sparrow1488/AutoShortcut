using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Enums;

public class RuleApply
{
    [JsonConstructor]
    private RuleApply(string type)
    {
        Type = type;
    }

    public string Type { get; }

    /// <summary>
    ///     This application type says that the processing file should always be applied when rendering <see cref="IProject"/>. Unable to save processed state for future recovery
    /// </summary>
    public static readonly RuleApply Runtime = new(nameof(Runtime));
    /// <summary>
    ///     This application type says that the rendered file (applied <see cref="IFileRule"/>) can be saved after rendering <see cref="IProject"/> and can be used again without it
    /// </summary>
    public static readonly RuleApply Permanent = new(nameof(Permanent));
}
