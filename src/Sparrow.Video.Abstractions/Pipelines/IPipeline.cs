using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Pipelines
{
    public interface IPipeline
    {
        IPipeline Configure(Action<IPipelineOptions> options);
        IProject CreateProject();
    }
}
