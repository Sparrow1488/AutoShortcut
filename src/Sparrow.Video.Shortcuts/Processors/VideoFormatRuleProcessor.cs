using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processors;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Rules;

namespace Sparrow.Video.Shortcuts.Processors
{
    public class VideoFormatRuleProcessor : IRuleProcessor<VideoFormatFileRule>
    {
        public VideoFormatRuleProcessor(
            IFormatorProcess formatorProcess)
        {
            _formatorProcess = formatorProcess;
        }

        private readonly IFormatorProcess _formatorProcess;

        public async Task ProcessAsync(IProjectFile file, VideoFormatFileRule rule)
        {
            var formattedFile = await _formatorProcess.CreateInFormatAsync(
                                    file.File, file.Analyse, rule.FormatSettings);
            file.References.Add(new Reference()
            {
                Name = RuleName.Formating.Value,
                FileFullPath = formattedFile.Path,
                Type = ReferenceType.InProcess
            });
        }
    }
}
