using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Pipelines.Options
{
    public interface IPipelineOptions
    {
        bool IsSerialize { get; set; }
        ICollection<IFileRule> Rules { get; }
        IEnumerable<Type> RulesTypes { get; }
        void AddRule<TRule>() where TRule : IFileRule;
    }
}
