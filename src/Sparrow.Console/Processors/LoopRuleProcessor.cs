using Sparrow.Console.Rules;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Processors;

namespace Sparrow.Console.Processors;

public class LoopRuleProcessor : RuleProcessorBase<LoopFileRuleBase>
{
    private readonly IUploadFilesService _uploadFilesService;

    public LoopRuleProcessor(IUploadFilesService uploadFilesService)
        => _uploadFilesService = uploadFilesService;

    public override ReferenceType ResultFileReferenceType => ReferenceType.RenderReady;

    public override Task<IFile> ProcessAsync(IProjectFile file, LoopFileRuleBase rule)
    {
        var processFileReference = file.References.GetActual();
        var processFile = _uploadFilesService.GetFile(processFileReference.FileFullPath);
        #region NOTE
        // NOTE: тут небольшой архитектурный прикол, я цепляю на файл ссылки, которые в будущем будут
        //       обрабатываться ffmpeg как "зацикливание". rule.LoopCount - это число зацикливаний
        //       В отличие от прочих обработчиков правил, это ничего не обрабатывает, а лишь служит
        //       для прикрепа ссылочек. Это полный кал говна, но короче пока что есть, мне впадлу это переписывать
        //       magicOne=1, тк после ретурна processFile в базовом классе ссылка цепляется еще раз.
        #endregion

        byte magicOne = 1;
        for (int i = 0; i < rule.LoopCount - magicOne; i++)
        {
            AddReference(
                toFile: file,
                appliedRule: rule,
                resultFile: processFile);
        }
        return Task.FromResult(processFile);
    }
}
