using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects.Options;
using Sparrow.Video.Shortcuts.Primitives.Structures;

namespace Sparrow.Video.Shortcuts.Projects.Options
{
    public class ProjectOptions : IProjectOptions
    {
        public ProjectOptions()
        {
            Structure = new NameStructure();
        }

        public IFilesStructure Structure { get; private set; }

        public IProjectOptions StructureBy(IFilesStructure structure)
        {
            Structure = structure;
            return this;
        }
    }
}
