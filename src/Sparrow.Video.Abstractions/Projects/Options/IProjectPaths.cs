namespace Sparrow.Video.Abstractions.Projects.Options;

public interface IProjectPaths
{
    string Get(string name);
    string GetRequired(string name);
    string GetRoot();
}
