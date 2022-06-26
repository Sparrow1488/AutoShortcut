using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Rules
{
    public abstract class FileRuleBase : IFileRule
    {
        public FileRuleBase(Func<IProjectFile, bool> condition)
        {
            Condition = condition;
        }

        public abstract RuleName RuleName { get; }
        public Func<IProjectFile, bool> Condition { get; }

        public bool IsInRule(IProjectFile file)
        {
            return Condition.Invoke(file);
        }
    }
}
