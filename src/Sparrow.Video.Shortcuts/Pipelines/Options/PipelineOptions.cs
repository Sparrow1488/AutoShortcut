using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Pipelines.Options
{
    public class PipelineOptions : IPipelineOptions
    {
        private readonly ICollection<Type> _rulesTypes = new List<Type>();

        public bool IsSerialize { get; set; }
        public ICollection<IFileRule> Rules { get; } = new List<IFileRule>();
        public IEnumerable<Type> RulesTypes => _rulesTypes;

        public void AddRule<TRule>() where TRule : IFileRule
        {
            _rulesTypes.Add(typeof(TRule));
        }
    }
}
