using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Pipelines.Options
{
    public class PipelineOptions : IPipelineOptions
    {
        public bool IsSerialize { get; set; }
        public ICollection<IFileRule> Rules { get; } = new List<IFileRule>();
    }
}
