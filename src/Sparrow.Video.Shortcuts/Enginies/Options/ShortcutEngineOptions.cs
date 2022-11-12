using Sparrow.Video.Abstractions.Enginies.Options;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Enginies.Options
{
    public class ShortcutEngineOptions : IShortcutEngineOptions
    {
        public ICollection<Type> ProcessorsTypes { get; } = new List<Type>();
        public IShortcutEngineOptions AddRuleProcessor<TProcessor, TRule>()
            where TProcessor : IRuleProcessor<TRule>
            where TRule : IFileRule
        {
            ProcessorsTypes.Add(typeof(TProcessor));
            return this;
        }
    }
}
