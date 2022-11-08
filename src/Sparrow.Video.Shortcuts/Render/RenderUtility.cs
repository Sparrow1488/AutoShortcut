using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Render;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Video.Shortcuts.Render;

public class RenderUtility : IRenderUtility
{
    private readonly ILogger<RenderUtility> _logger;
    private readonly IRuleProcessorsProvider _ruleProcessorsProvider;
    private readonly ITextFormatter _textFormatter;
    private readonly IProjectSerializationService _projectSerialization;
    private readonly IPathsProvider _pathsProvider;
    private readonly IProjectSaveSettingsCreator _saveSettingsCreator;
    private readonly IConcatinateProcess _concatinateProcess;

    private IProjectFile? _loggedProcessingFile;

    public RenderUtility(
        ILogger<RenderUtility> logger,
        IRuleProcessorsProvider ruleProcessorsProvider,
        ITextFormatter textFormatter,
        IProjectSerializationService projectSerialization,
        IPathsProvider pathsProvider,
        IProjectSaveSettingsCreator saveSettingsCreator,
        IConcatinateProcess concatinateProcess)
    {
        _logger = logger;
        _ruleProcessorsProvider = ruleProcessorsProvider;
        _textFormatter = textFormatter;
        _projectSerialization = projectSerialization;
        _pathsProvider = pathsProvider;
        _saveSettingsCreator = saveSettingsCreator;
        _concatinateProcess = concatinateProcess;
    }

    public IProjectFile CurrentProcessFile { get; private set; }
    public IFileRule CurrentApplyingRule { get; private set; }
    private ProcessingFilesStatistic FilesStatistic { get; set; }

    private string CurrentProcessingFileLog => $"({FilesStatistic.CurrentIndexProcessed + 1}/{FilesStatistic.TotalFiles}) ";

    public async Task<IFile> StartRenderAsync(
        IProject project, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting render project");
        await _projectSerialization.SaveProjectOptionsAsync(project);
        _logger.LogInformation("Total shortcut files {count}", project.Files.Count());

        await _projectSerialization.SaveProjectFilesAsync(project.Files);

        var filesArray = project.Files.ToArray();
        foreach (var file in filesArray)
        {
            if (!file.RulesContainer.Any())
            {
                _logger.LogWarning(
                    "File {file} no contains any processing rule",
                    _textFormatter.GetPrintable(file.File.Name));
                continue;
            }
                
            foreach (var rule in file.RulesContainer)
            {
                CurrentProcessFile = file;
                CurrentApplyingRule = rule;
                FilesStatistic = new(filesArray.Length, Array.IndexOf(filesArray, file));
                await ApplyFileRuleAsync(cancellationToken);
            }
        }
        var concatinateFilesPaths = GetConcatinateFilesPaths(project.Files);

        var saveSettings = _saveSettingsCreator.Create(
                                sectionName: "ResultFiles", 
                                fileName:     project.Options.ProjectName + ".mp4");
        var result = await _concatinateProcess.ConcatinateFilesAsync(
                        concatinateFilesPaths, saveSettings, cancellationToken);
        return result;
    }

    private async Task ApplyFileRuleAsync(CancellationToken cancellationToken = default)
    {
        PrintCurrentApplyingRuleLog();
        if (IsCurrentFileRuleNotAppliedOrRuntimeProcessing())
        {
            var processor = (IRuleProcessor)_ruleProcessorsProvider.GetRuleProcessor(CurrentApplyingRule.GetType());
            await processor.ProcessAsync(CurrentProcessFile, CurrentApplyingRule, cancellationToken);
            CurrentApplyingRule.Applied();
            await _projectSerialization.SaveProjectFileAsync(CurrentProcessFile);
        }
        else
        {
            _logger.LogInformation(
                CurrentProcessingFileLog.MakeEmpty() + 
                "Rule '{rule}' is already applied for {shortFileName}",
                CurrentApplyingRule.RuleName.Value,
                _textFormatter.GetPrintable(CurrentProcessFile.File.Name));
        }
    }

    private bool IsCurrentFileRuleNotAppliedOrRuntimeProcessing()
        => !CurrentApplyingRule.IsApplied || CurrentApplyingRule.RuleApply == RuleApply.Runtime;

    private void PrintCurrentApplyingRuleLog()
    {
        if (_loggedProcessingFile != CurrentProcessFile)
            _loggedProcessingFile = default;

        string logPrefix = _loggedProcessingFile is null ? CurrentProcessingFileLog 
                                : CurrentProcessingFileLog.MakeEmpty();
        _logger.LogInformation(
            logPrefix + "Applying {ruleType} rule '{rule}' for {fileShortName}",
            CurrentApplyingRule.RuleApply.Type,
            CurrentApplyingRule.RuleName.Value,
            _textFormatter.GetPrintable(CurrentProcessFile.File.Name));

        _loggedProcessingFile ??= CurrentProcessFile;
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
            _logger.LogInformation($"Original file \"{_textFormatter.GetPrintable(file.File.Name)}\" will concatinate {renderPaths.Count} times");
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
