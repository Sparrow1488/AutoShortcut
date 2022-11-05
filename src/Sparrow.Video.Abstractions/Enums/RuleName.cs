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
    public static readonly RuleName Group = new(nameof(Group));
    public static readonly RuleName Formating = new(nameof(Formating));
    public static readonly RuleName Encoding = new(nameof(Encoding));
    public static readonly RuleName Silent = new(nameof(Silent));

    public static RuleName New(string name)
    {
        // TODO: проверка входа
        return new RuleName(name);
    }
}
