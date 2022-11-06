using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Abstractions.Rules;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;

namespace Sparrow.Video.Shortcuts.Processors;

public abstract class RuleProcessorBase<TRule> : IRuleProcessor<TRule>
    where TRule : IFileRule
{
    public RuleProcessorBase(
        IUploadFilesService uploadFilesService)
    {
        UploadFilesService = uploadFilesService;
    }

    public abstract ReferenceType ResultFileReferenceType { get; }
    public IUploadFilesService UploadFilesService { get; }

    public abstract Task<IFile> ProcessAsync(IProjectFile file, TRule rule, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Apply specific processing file rule to <paramref name="file"/>. 
    ///     Add reference to processed file path.
    /// </summary>
    /// <param name="file">File being processed</param>
    /// <param name="rule">Special rule handler</param>
    /// <returns>Processed file result</returns>
    public async Task<IFile> ProcessAsync(IProjectFile file, IFileRule rule, CancellationToken cancellationToken = default)
    {
        if (!IsInputAndCurrentRulesAreTheSame(input: rule))
            throw new FailedProcessRuleException("Processor can't handle current rule type " + rule.RuleName.Value);

        var processedResultFile = await ProcessAsync(file, (TRule)rule, cancellationToken);
        AddReference(
            toFile: file,
            appliedRule: rule,
            resultFile: processedResultFile);
        return processedResultFile;
    }

    private bool IsInputAndCurrentRulesAreTheSame(IFileRule input)
        => input.GetType().IsAssignableTo(GetRuleType()) || input.GetType() == GetRuleType();

    public Type GetRuleType() => typeof(TRule);

    protected void AddReference(IProjectFile toFile, IFileRule appliedRule, IFile resultFile)
    {
        toFile.References.Add(new Reference()
        {
            Name = appliedRule.RuleName.Value,
            Type = ResultFileReferenceType,
            FileFullPath = resultFile.Path
        });
    }

    protected IFile GetActualFile(IProjectFile projectFile)
    {
        var actualFilePath = projectFile.References.GetActual().FileFullPath;
        return UploadFilesService.GetFile(actualFilePath);
    }
}
