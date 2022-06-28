using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enginies;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Pipelines;
using Sparrow.Video.Abstractions.Pipelines.Options;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Builders;
using Sparrow.Video.Shortcuts.Enums;
using Sparrow.Video.Shortcuts.Pipelines;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Rules;
using Sparrow.Video.Shortcuts.Services.Options;
using File = Sparrow.Video.Shortcuts.Primitives.File;

namespace Sparrow.Video.Shortcuts.Enginies
{
    public class ShortcutEngine : IShortcutEngine
    {
        public ShortcutEngine(
            ILogger<ShortcutEngine> logger,
            IUploadFilesService uploadFilesService,
            IAnalyseProcess analyseProcess,
            IFormatorProcess formatorProcess,
            ISaveService saveService,
            IStoreService storeService,
            IServiceProvider services,
            IRuleProcessorsProvider ruleProcessorsProvider,
            IPathsProvider pathsProvider)
        {
            _logger = logger;
            _uploadFilesService = uploadFilesService;
            _analyseProcess = analyseProcess;
            _formatorProcess = formatorProcess;
            _saveService = saveService;
            _services = services;
            _ruleProcessorsProvider = ruleProcessorsProvider;
            _pathsProvider = pathsProvider;
            _storeService = storeService;
        }

        private readonly ILogger<ShortcutEngine> _logger;
        private readonly IUploadFilesService _uploadFilesService;
        private readonly IAnalyseProcess _analyseProcess;
        private readonly IFormatorProcess _formatorProcess;
        private readonly ISaveService _saveService;
        private readonly IServiceProvider _services;
        private readonly IRuleProcessorsProvider _ruleProcessorsProvider;
        private readonly IPathsProvider _pathsProvider;
        private readonly IStoreService _storeService;

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
                var projectFile = new ProjectFile()
                {
                    File = file,
                    Analyse = fileAnalyse,
                };
                projectFile.References.Add(new Reference() 
                {
                    Name = "Original",
                    FileFullPath = projectFile.File.Path,
                    Type = ReferenceType.OriginalSource
                });
                projectFilesList.Add(projectFile);
                _logger.LogInformation($"[Analyse({i + 1}/{files.Count})] Completed");
                await SaveProjectFileAsync(projectFile);
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

        private async Task SaveProjectFileAsync(
            IProjectFile projectFile, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[SaveService] Saving project file");
            var saveOptions = new SaveOptions()
            {
                Id = Guid.NewGuid(),
                Name = projectFile.File.Name,
                DirectoryPath = _pathsProvider.GetPathFromCurrent(PathName.Files)
            }; // TODO: нужно иметь Engine options, что указывать коренную папку для сохранения
            await _saveService.SaveAsync(projectFile, saveOptions, cancellationToken);
            _logger.LogInformation("[SaveService] Saved success");

            // TODO: не робит
            //var saved = await _storeService.GetObjectAsync<ProjectFile>(projectFile.File.Name);
        }

        public async Task<IFile> StartRenderAsync(
            IProject project, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting render project");
            _logger.LogInformation($"Total shortcut files => {project.Files.Count()}");
            var scriptBuilder = new ScriptBuilder();
            foreach (var file in project.Files)
                foreach (var rule in file.RulesCollection)
                {
                    var processor = (IRuleProcessor)_ruleProcessorsProvider.GetRuleProcessor(rule.GetType());
                    await processor.ProcessAsync(file, rule);
                }
            return new File();
        }

        private async Task StartRuleProcessAsync<TRule>(IProjectFile file, TRule rule)
            where TRule : IFileRule
        {
            _logger.LogInformation($"Looking up processor for Rule - {rule.GetType().Name}");
            var processor = (IRuleProcessor<TRule>)_services.GetService(typeof(IRuleProcessor<TRule>));
            if (processor is not null)
            {
                _logger.LogInformation($"Starting {processor.GetType().Name} process");
                await processor.ProcessAsync(file, rule);
            }
            else
            {
                _logger.LogWarning($"Processor not found");
            }
        }
    }
}
