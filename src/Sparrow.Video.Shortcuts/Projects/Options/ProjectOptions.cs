using Newtonsoft.Json;
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

        [JsonProperty]
        public IFilesStructure Structure { get; private set; }
        [JsonProperty]
        public string ProjectName { get; private set; } = "ProjectName";

        public IProjectOptions Named(string name)
        {
            ProjectName = name.Trim();
            return this;
        }

        public IProjectOptions StructureBy(IFilesStructure structure)
        {
            Structure = structure;
            return this;
        }
    }
}
