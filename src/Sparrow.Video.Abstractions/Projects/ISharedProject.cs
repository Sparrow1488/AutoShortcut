namespace Sparrow.Video.Abstractions.Projects;

public interface ISharedProject
{
    IProject Project { get; set; }
    bool IsInit();
    void Assert();
}
