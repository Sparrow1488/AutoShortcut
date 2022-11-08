using Newtonsoft.Json;

namespace Sparrow.Video.Abstractions.Enums;

[Serializable]
public class RuleName
{
    [JsonConstructor]
    private RuleName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly RuleName Loop = new(nameof(Loop));
    public static readonly RuleName Encoding = new(nameof(Encoding));
    public static readonly RuleName Scale = new(nameof(Scale));
    public static readonly RuleName Silent = new(nameof(Silent));
    public static readonly RuleName Snapshot = new(nameof(Snapshot));

    public static RuleName New(string name)
    {
        return new RuleName(name);
    }
}
