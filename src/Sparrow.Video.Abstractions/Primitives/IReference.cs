namespace Sparrow.Video.Abstractions.Primitives
{
    public interface IReference
    {
        string Name { get; }
        string FileFullPath { get; }
        string Target { get; }
    }
}
