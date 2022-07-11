using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Projects
{
    public interface IProject
    {
        string Name { get; }
        IScript RenderScript { get; }
        IEnumerable<IProjectFile> Files { get; }
        IProject Named(string projectName);
    }
}
