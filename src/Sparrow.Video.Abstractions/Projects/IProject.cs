using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Projects
{
    public interface IProject
    {
        IScript RenderScript { get; }
        IEnumerable<IProjectFile> Files { get; }
    }
}
