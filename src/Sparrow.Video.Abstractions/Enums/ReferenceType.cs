namespace Sparrow.Video.Abstractions.Enums;

public class ReferenceType
{
    [Newtonsoft.Json.JsonConstructor]
    private ReferenceType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly ReferenceType RenderReady = new(nameof(RenderReady));
    public static readonly ReferenceType OriginalSource = new(nameof(OriginalSource));
    public static readonly ReferenceType InProcess = new(nameof(InProcess));
    public static readonly ReferenceType Ignore = new(nameof(Ignore));
}