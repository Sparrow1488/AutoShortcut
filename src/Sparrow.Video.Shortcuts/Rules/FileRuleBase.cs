using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Rules
{
    public abstract class FileRuleBase : IFileRule
    {
        [JsonProperty]
        public abstract RuleName RuleName { get; }
        [JsonIgnore]
        public abstract Func<IProjectFile, bool> Condition { get; }
        [JsonProperty]
        public bool IsApplied { get; set; }

        public void Applied() => IsApplied = true;
        public bool IsInRule(IProjectFile file) => Condition.Invoke(file);
    }
}
