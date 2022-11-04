using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processes.Settings;
using System.Text;

namespace Sparrow.Video.Shortcuts.Render;

public class RenderUtility : IRenderUtility
{
    private readonly ILogger<RenderUtility> _logger;
    private readonly IRuleProcessorsProvider _ruleProcessorsProvider;
    private readonly ITextFormatter _textFormatter;
    private readonly ISaveService _saveService;
    private readonly IJsonSerializer _serializer;
    private readonly IPathsProvider _pathsProvider;
    private readonly IConcatinateProcess _concatinateProcess;

    private IProjectFile? _loggedProcessingFile;

    public RenderUtility(
        ILogger<RenderUtility> logger,
        IRuleProcessorsProvider ruleProcessorsProvider,
        ITextFormatter textFormatter,
        ISaveService saveService,
        IJsonSerializer serializer,
        IPathsProvider pathsProvider,
        IConcatinateProcess concatinateProcess)
    {
        _logger = logger;
        _ruleProcessorsProvider = ruleProcessorsProvider;
        _textFormatter = textFormatter;
        _saveService = saveService;
        _serializer = serializer;
        _pathsProvider = pathsProvider;
        _concatinateProcess = concatinateProcess;
    }

    public IProjectFile CurrentProcessFile { get; private set; }
    public IFileRule CurrentApplyingRule { get; private set; }
    private ProcessingFilesStatistic FilesStatistic { get; set; }

    public async Task<IFile> StartRenderAsync(
        IProject project, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting render project");
        await SaveProjectOptionsAsync(project);
        _logger.LogInformation("Total shortcut files {count}", project.Files.Count());

        await SaveProjectFilesAsync(project.Files);

        var filesArray = project.Files.ToArray();
        foreach (var file in filesArray)
            foreach (var rule in file.RulesCollection)
            {
                CurrentProcessFile = file;
                CurrentApplyingRule = rule;
                FilesStatistic = new(filesArray.Length, Array.IndexOf(filesArray, file));
                await ApplyFileRuleAsync();
            }
        var concatinateFilesPaths = GetConcatinateFilesPaths(project.Files);
       
        var result = await _concatinateProcess.ConcatinateFilesAsync(concatinateFilesPaths, saveSettings);
        return result;
    }

    private async Task SaveProjectOptionsAsync(IProject project)
    {
        var serializedOptions = _serializer.Serialize(project.Options);
        var saveProjectOptionsPath = _pathsProvider.GetPathFromCurrent("ProjectOptions");
        var saveSettings = new SaveSettings()
        {
            SaveFullPath = Path.Combine(saveProjectOptionsPath, "project-options.json")
        };
        await _saveService.SaveTextAsync(serializedOptions, saveSettings);
    }

    private async Task ApplyFileRuleAsync()
    {
        if (IsCurrentFileRuleNotAppliedOrRuntimeProcessing())
        {
            PrintCurrentApplyingRuleLog();
            var processor = (IRuleProcessor)_ruleProcessorsProvider.GetRuleProcessor(CurrentApplyingRule.GetType());
            await processor.ProcessAsync(CurrentProcessFile, CurrentApplyingRule);
            CurrentApplyingRule.Applied();
            await SaveProjectFileAsync(CurrentProcessFile);
        }
        else
        {
            _logger.LogInformation($"Rule named \"{CurrentApplyingRule.RuleName.Value}\" is already applied for {_textFormatter.GetPrintable(CurrentProcessFile.File.Name)}");
        }
    }

    private bool IsCurrentFileRuleNotAppliedOrRuntimeProcessing()
        => !CurrentApplyingRule.IsApplied || CurrentApplyingRule.RuleApply == RuleApply.Runtime;

    private void PrintCurrentApplyingRuleLog()
    {
        if (_loggedProcessingFile != CurrentProcessFile)
            _loggedProcessingFile = default;

        var processingFileNumeric = $"({FilesStatistic.CurrentIndexProcessed + 1}/{FilesStatistic.TotalFiles})";
        string logText = $" Applying {CurrentApplyingRule.RuleApply.Type} rule \"{CurrentApplyingRule.RuleName.Value}\" for {_textFormatter.GetPrintable(CurrentProcessFile.File.Name)}";
        if (_loggedProcessingFile is null) // numeric not printed
        {
            _logger.LogInformation(processingFileNumeric + logText);
        }
        else                               // numeric printed
        {
            var stringBuilder = new StringBuilder();
            var buildedLog = stringBuilder.Append(' ', processingFileNumeric.Length).Append(logText).ToString();
            _logger.LogInformation(buildedLog);
        }
        
        _loggedProcessingFile ??= CurrentProcessFile;
    }

    private async Task SaveProjectFilesAsync(IEnumerable<IProjectFile> files)
    {
        foreach (var file in files)
            await SaveProjectFileAsync(file);
    }

    private async Task SaveProjectFileAsync(IProjectFile file)
    {
        var saveSettings = new SaveSettings() {
            SaveFullPath = file.File.Path.ChangeFileExtension(setExtension: ".restore")
        };
        var serializedFile = _serializer.Serialize(file);
        await _saveService.SaveTextAsync(serializedFile, saveSettings);
    }

    private IEnumerable<string> GetConcatinateFilesPaths(IEnumerable<IProjectFile> files)
    {
        var renderPathsList = new List<string>();
        foreach (var file in files)
        {
            var renderPaths = file.References.Where(x => x.Type == ReferenceType.RenderReady)
                                                .Select(x => x.FileFullPath)
                                                    .ToList();
            if (!renderPaths.Any())
            {
                _logger.LogDebug($"File \"{file.File.Name}\" not contains any reference to render file. Use actual");
                renderPaths.Add(file.References.GetActual().FileFullPath);
            }
            _logger.LogInformation($"Original file \"{_textFormatter.GetPrintable(file.File.Name)}\" will concatinate {renderPaths.Count}");
            renderPathsList.AddRange(renderPaths);
        }
        return renderPathsList;
    }

    private struct ProcessingFilesStatistic
    {
        public ProcessingFilesStatistic(int total, int current)
        {
            TotalFiles = total;
            CurrentIndexProcessed = current;
        }

        public int TotalFiles { get; }
        public int CurrentIndexProcessed { get; }
    }
}
