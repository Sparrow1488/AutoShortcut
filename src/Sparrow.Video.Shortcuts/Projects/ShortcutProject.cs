using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Shortcuts.Exceptions;

namespace Sparrow.Video.Shortcuts.Projects
{
    public class ShortcutProject : IProject
    {
        public string Name { get; private set; }
        public IEnumerable<IProjectFile> Files { get; set; }

        public IProject Named(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                throw new EmptyOrNullArgumentException("Failed to set null or empty name to ad project");
            Name = projectName;
            return this;
        }
    }
}
