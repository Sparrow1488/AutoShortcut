namespace Sparrow.Video.Abstractions.Services.Options
{
    public interface ISaveOptions
    {
        Guid Id { get; }
        string Name { get; }
        string DirectoryPath { get; }
    }
}
