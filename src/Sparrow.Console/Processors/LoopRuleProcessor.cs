using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors
{
    public class LoopRuleProcessor : RuleProcessorBase<LoopFileRuleBase>
    {
        public LoopRuleProcessor(
            IUploadFilesService uploadFilesService)
        {
            _uploadFilesService = uploadFilesService;
        }

        private readonly IUploadFilesService _uploadFilesService;

        public override Task ProcessAsync(IProjectFile file, LoopFileRuleBase rule)
        {
            var processFileReference = file.References.GetActual();
            var processFile = _uploadFilesService.GetFile(processFileReference.FileFullPath);
            for (int i = 0; i < rule.LoopCount; i++)
            {
                file.References.Add(new Reference()
                {
                    Name = rule.RuleName.Value,
                    FileFullPath = processFile.Path,
                    Type = ReferenceType.RenderReady
                });
            }
            return Task.CompletedTask;
        }
    }
}
