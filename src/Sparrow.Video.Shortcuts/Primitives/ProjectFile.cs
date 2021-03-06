using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Primitives
{
    public class ProjectFile : IProjectFile
    {
        [JsonProperty]
        public IFile File { get; internal set; }
        [JsonProperty]
        public IFileAnalyse Analyse { get; internal set; }
        public ICollection<IFileRule> RulesCollection { get; } = new List<IFileRule>();
        public ICollection<IReference> References { get; } = new List<IReference>();
    }
}
