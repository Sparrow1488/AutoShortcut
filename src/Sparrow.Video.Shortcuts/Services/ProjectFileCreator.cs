using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Primitives;

namespace Sparrow.Video.Shortcuts.Services
{
    public class ProjectFileCreator : IProjectFileCreator
    {
        public ProjectFileCreator(
            IAnalyseProcess analyseProcess)
        {
            _analyseProcess = analyseProcess;
        }

        private readonly IAnalyseProcess _analyseProcess;

        public async Task<IProjectFile> CreateAsync(IFile file, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fileAnalyse = await _analyseProcess.GetAnalyseAsync(file); // TODO: сюда токен
            var projectFile = new ProjectFile() {
                File = file,
                Analyse = fileAnalyse,
            };
            projectFile.References.Add(new Reference() {
                Name = "Original",
                FileFullPath = projectFile.File.Path,
                Type = ReferenceType.OriginalSource
            });
            return projectFile;
        }
    }
}
