using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Shortcuts.Projects
{
    public class ShortcutProject : IProject
    {
        public ShortcutProject(IProjectOptions options)
        {
            Options = options;
        }

        public string Name => Options.ProjectName;
        public IEnumerable<IProjectFile> Files { get; set; }
        public IProjectOptions Options { get; internal set; }

        public IProject ConfigureOptions(Action<IProjectOptions> configureOptions)
        {
            configureOptions.Invoke(Options);
            return this;
        }
    }
}
