using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Video.Shortcuts.Pipelines;

public class ShortcutPipeline : IShortcutPipeline
{
    private readonly IPipelineOptions _options;
    private readonly IProjectCreator _projectCreator;

    public ShortcutPipeline(
        IPipelineOptions options,
        IProjectCreator projectCreator)
    {
        _options = options;
        _projectCreator = projectCreator;
    }

    public ICollection<IProjectFile> ProjectFiles { get; set; } = new List<IProjectFile>();

    public IPipeline Configure(Action<IPipelineOptions> options)
    {
        options.Invoke(_options);
        return this;
    }

    public IProject CreateProject()
        => CreateProject(opt => opt.StructureBy(new NameStructure()));

    public IProject CreateProject(Action<IProjectOptions> options)
    {
        return _projectCreator.CreateProject(ProjectFiles, options);
    }

    public IShortcutPipeline SetFiles(IEnumerable<IProjectFile> files)
    {
        ProjectFiles = files.ToArray();
        return this;
    }
}
