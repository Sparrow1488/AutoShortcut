using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Pipelines.Options;

public interface IPipelineOptions
{
    bool IsSerialize { get; set; }
    IEnumerable<RuleStore> Stores { get; }

    void AddRule<TRule>() where TRule : IFileRule;
    void AddRule(IFileRule rule);
}
