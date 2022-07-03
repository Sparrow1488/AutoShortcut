using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives.Structures
{
    public class GroupStructure : IFilesStructure
    {
        public GroupStructure()
        {
            FilesStructure = new NameStructure();
        }

        public GroupStructure(ILogger logger) : this()
        {
            _logger = logger;
        }

        private readonly ILogger? _logger;

        public IFilesStructure FilesStructure { get; private set; }

        public IEnumerable<IProjectFile> GetStructuredFiles(IEnumerable<IProjectFile> files)
        {
            var structuredFilesList = new List<IProjectFile>();
            var groups = files.GroupBy(x => x.File.Group);
            _logger?.LogInformation($"Founded {groups.Count()} files groups");
            foreach (var group in groups)
            {
                var groupFiles = group.ToArray();
                var structuredGroupFiles = FilesStructure.GetStructuredFiles(groupFiles);
                structuredFilesList.AddRange(structuredGroupFiles);
                _logger?.LogInformation($"Structured {groupFiles.Count()} files in {group.Key} group");
            }
            return structuredFilesList;
        }

        public GroupStructure StructureFilesBy(IFilesStructure structure)
        {
            FilesStructure = structure;
            return this;
        }
    }
}
