using Microsoft.Extensions.Logging;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Projects;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;

namespace Sparrow.Video.Shortcuts.Render;

public class RenderUtility : IRenderUtility
{
    public RenderUtility(
        ILogger<RenderUtility> logger,
        IRuleProcessorsProvider ruleProcessorsProvider,
        ITextFormatter textFormatter,
        IConcatinateProcess concatinateProcess)
    {
        _logger = logger;
        _ruleProcessorsProvider = ruleProcessorsProvider;
        _textFormatter = textFormatter;
        _concatinateProcess = concatinateProcess;
    }

    private readonly ILogger<RenderUtility> _logger;
    private readonly IRuleProcessorsProvider _ruleProcessorsProvider;
    private readonly ITextFormatter _textFormatter;
    private readonly IConcatinateProcess _concatinateProcess;

    public async Task<IFile> StartRenderAsync(
        IProject project, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting render project");
        _logger.LogInformation($"Total shortcut files => {project.Files.Count()}");
        foreach (var file in project.Files)
            foreach (var rule in file.RulesCollection)
            {
                _logger.LogInformation($"Applying rule \"{rule.RuleName.Value}\" " +
                    $"for {_textFormatter.GetPrintable(file.File.Name)}");
                var processor = (IRuleProcessor)_ruleProcessorsProvider.GetRuleProcessor(rule.GetType());
                await processor.ProcessAsync(file, rule);
            }
        var concatinateFilesPaths = GetConcatinateFilesPaths(project.Files);
       
        var result = await _concatinateProcess.ConcatinateFilesAsync(concatinateFilesPaths, saveSettings);
        return result;
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
            _logger.LogInformation($"Original file \"{file.File.Name}\" will concatinate {renderPaths.Count}");
            renderPathsList.AddRange(renderPaths);
        }
        return renderPathsList;
    }
}
