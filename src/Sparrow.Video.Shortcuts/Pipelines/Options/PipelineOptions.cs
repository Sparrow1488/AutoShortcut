using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Pipelines.Options;

public class PipelineOptions : IPipelineOptions
{
    public readonly List<RuleStore> _stores = new();

    public PipelineOptions() 
    {
        
    }

    public bool IsSerialize { get; set; }
    public IEnumerable<RuleStore> Stores => _stores;

    public void AddRule<TRule>() 
        where TRule : IFileRule
            => _stores.Add(new(typeof(TRule)));

    public void AddRule(IFileRule rule)
        => _stores.Add(new(rule));
}
