using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Shortcuts.Projects
{
    public class ShortcutProject : IProject
    {
        public IScript RenderScript { get; set; }
        public IEnumerable<IProjectFile> Files { get; set; }
    }
}
