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

namespace Sparrow.Video.Shortcuts.Render;

public class RenderUtility : IRenderUtility
{
    public RenderUtility(
        ILogger<RenderUtility> logger,
        IRuleProcessorsProvider ruleProcessorsProvider,
        ITextFormatter textFormatter,
        ISaveService saveService,
        IJsonSerializer serializer,
        IConcatinateProcess concatinateProcess)
    {
        _logger = logger;
        _ruleProcessorsProvider = ruleProcessorsProvider;
        _textFormatter = textFormatter;
        _saveService = saveService;
        _serializer = serializer;
        _concatinateProcess = concatinateProcess;
    }

    private readonly ILogger<RenderUtility> _logger;
    private readonly IRuleProcessorsProvider _ruleProcessorsProvider;
    private readonly ITextFormatter _textFormatter;
    private readonly ISaveService _saveService;
    private readonly IJsonSerializer _serializer;
    private readonly IConcatinateProcess _concatinateProcess;

    public async Task<IFile> StartRenderAsync(
        IProject project, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting render project");
        _logger.LogInformation($"Total shortcut files => {project.Files.Count()}");
        foreach (var file in project.Files)
            foreach (var rule in file.RulesCollection)
                await ApplyFileRuleAsync(file, rule);
        var concatinateFilesPaths = GetConcatinateFilesPaths(project.Files);
       
        var result = await _concatinateProcess.ConcatinateFilesAsync(concatinateFilesPaths, saveSettings);
        return result;
    }

    private async Task ApplyFileRuleAsync(IProjectFile file, IFileRule rule)
    {
        if (!rule.IsApplied)
        {
            _logger.LogInformation($"Applying rule \"{rule.RuleName.Value}\" " +
                                   $"for {_textFormatter.GetPrintable(file.File.Name)}");
            var processor = (IRuleProcessor)_ruleProcessorsProvider.GetRuleProcessor(rule.GetType());
            await processor.ProcessAsync(file, rule);
            rule.Applied();
            await SaveProjectFileAsync(file);
        }
        else
        {
            _logger.LogInformation($"Rule named \"{rule.RuleName.Value}\" is already applied for {_textFormatter.GetPrintable(file.File.Name)}");
        }
    }

    private async Task SaveProjectFileAsync(IProjectFile file)
    {
        var saveSettings = new SaveSettings()
        {
            SaveFullPath = Path.Combine(Path.GetDirectoryName(file.File.Path), file.File.Name + ".restore")
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
}
