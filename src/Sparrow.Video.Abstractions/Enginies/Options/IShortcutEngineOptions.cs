using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Enginies.Options
{
    public interface IShortcutEngineOptions
    {
        IShortcutEngineOptions AddRuleProcessor<TProcessor, TRule>()
            where TProcessor : IRuleProcessor<TRule>
                where TRule : IFileRule;
    }
}
