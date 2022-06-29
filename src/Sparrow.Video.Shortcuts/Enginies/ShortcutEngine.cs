using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Pipelines;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Processes.Settings;
using Sparrow.Video.Shortcuts.Render;

namespace Sparrow.Video.Shortcuts.Enginies
{
    public class ShortcutEngine : IShortcutEngine
    {
        public ShortcutEngine(
            ILogger<ShortcutEngine> logger,
            IUploadFilesService uploadFilesService,
            IAnalyseProcess analyseProcess,
            IServiceProvider services,
            IPathsProvider pathsProvider,
            IRenderUtility renderUtility)
        {
            _logger = logger;
            _uploadFilesService = uploadFilesService;
            _analyseProcess = analyseProcess;
            _services = services;
            _pathsProvider = pathsProvider;
            _renderUtility = renderUtility;
        }

        private readonly ILogger<ShortcutEngine> _logger;
        private readonly IUploadFilesService _uploadFilesService;
        private readonly IAnalyseProcess _analyseProcess;
        private readonly IServiceProvider _services;
        private readonly IPathsProvider _pathsProvider;
        private readonly IRenderUtility _renderUtility;

        private ShortcutPipeline ShortcutPipeline { get; set; }

        public async Task<IShortcutPipeline> CreatePipelineAsync(
            string filesDirectory, CancellationToken cancellationToken = default)
        {
            CreateShortcutPipeline();
            var files = await _uploadFilesService.GetFilesAsync(filesDirectory, cancellationToken);
            _logger.LogInformation($"Uploaded {files.Count} files from directory");
            _logger.LogInformation("Starting analyse files");
            var projectFilesList = new List<IProjectFile>();
            for (int i = 0; i < files.Count; i++)
            {
                IFile file = files.ElementAt(i);
                _logger.LogInformation($"[Analyse({i + 1}/{files.Count})] Current: {file.Name}"); // service for print short name
                cancellationToken.ThrowIfCancellationRequested();
                var fileAnalyse = await _analyseProcess.GetAnalyseAsync(file);
                var projectFile = new ProjectFile() {
                    File = file,
                    Analyse = fileAnalyse,
                };
                projectFile.References.Add(new Reference() {
                    Name = "Original",
                    FileFullPath = projectFile.File.Path,
                    Type = ReferenceType.OriginalSource
                });
                projectFilesList.Add(projectFile);
                _logger.LogInformation($"[Analyse({i + 1}/{files.Count})] Completed");
            }
            _logger.LogInformation($"{files.Count} file analyse completed");
            _logger.LogInformation("Creating pipeline");
            ShortcutPipeline.ProjectFiles = projectFilesList;
            return ShortcutPipeline;
        }

        private void CreateShortcutPipeline()
        {
            var pipelineOptions = _services.GetRequiredService<IPipelineOptions>();
            ShortcutPipeline = new ShortcutPipeline(pipelineOptions);
        }

        public async Task<IFile> StartRenderAsync(
            IProject project, CancellationToken cancellationToken = default)
        {
            var resultSaveSettings = new SaveSettings() {
                SaveFullPath = Path.Combine(
                    _pathsProvider.GetPathFromCurrent("ResultFiles"), "Compilation.mp4")
            };
            return await _renderUtility.StartRenderAsync(project, resultSaveSettings);
        }
    }
}
