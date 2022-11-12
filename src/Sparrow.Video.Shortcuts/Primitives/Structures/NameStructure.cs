using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives.Structures
{
    public class NameStructure : IFilesStructure
    {
        public IEnumerable<IProjectFile> GetStructuredFiles(IEnumerable<IProjectFile> files)
        {
            return files.OrderBy(x => x.File.Name);
        }
    }
}
