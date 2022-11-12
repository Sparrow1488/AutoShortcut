using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Pipelines.Options;

public class RuleStore
{
    public RuleStore(Type type)
    {
        Kind = RuleStoreKind.Type;
        RuleType = type;
        Instance = null;
    }

    public RuleStore(IFileRule instance)
    {
        Kind = RuleStoreKind.Instance;
        RuleType = null;
        Instance = instance;
    }

    public RuleStoreKind Kind { get; }
    public Type? RuleType { get; }
    public IFileRule? Instance { get; }
}
