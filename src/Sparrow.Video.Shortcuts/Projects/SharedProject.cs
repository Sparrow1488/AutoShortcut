using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Shortcuts.Projects;

public class SharedProject : ISharedProject
{
    public IProject Project { get; set; }

    public bool IsInit() => Project is not null;
    public void Assert()
    {
        if (!IsInit())
        {
            throw new Exception("No shared project");
        }
    }
}
