using Sparrow.Video.Abstractions.Services.Options;

namespace Sparrow.Video.Shortcuts.Services.Options
{
    public class SaveOptions : ISaveOptions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DirectoryPath { get; set; }
    }
}
