using Newtonsoft.Json;
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

        [JsonProperty]
        public abstract RuleName RuleName { get; }
        [JsonIgnore]
        public Func<IProjectFile, bool> Condition { get; }
        [JsonProperty]
        public bool IsApplied { get; set; }

        public void Applied()
        {
            IsApplied = true;
        }

        public bool IsInRule(IProjectFile file)
        {
            return Condition.Invoke(file);
        }
    }
}
