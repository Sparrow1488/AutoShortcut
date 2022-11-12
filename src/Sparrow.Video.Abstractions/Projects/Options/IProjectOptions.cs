using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Projects.Options
{
    public interface IProjectOptions
    {
        string ProjectName { get; }
        IFilesStructure Structure { get; }
        IProjectOptions StructureBy(IFilesStructure structure);
        IProjectOptions Named(string name);
    }
}
