using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Video.Shortcuts.Primitives.Structures
{
    public class DurationStructure : IFilesStructure
    {
        private bool _descending = false;

        public IEnumerable<IProjectFile> GetStructuredFiles(IEnumerable<IProjectFile> files)
        {
            if (_descending) {
                return files.OrderByDescending(x => x.Analyse.StreamAnalyses.Video().Duration);
            }
            return files.OrderBy(x => x.Analyse.StreamAnalyses.Video().Duration);
        }

        public DurationStructure Descending()
        {
            _descending = true;
            return this;
        }
    }
}
