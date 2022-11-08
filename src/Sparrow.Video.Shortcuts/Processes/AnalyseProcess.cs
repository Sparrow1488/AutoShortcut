using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes;

public class AnalyseProcess : ExecutionProcessBase, IAnalyseProcess
{
    private StringPath _analyseFilePath;

    public AnalyseProcess(
        IServiceProvider services,
        IJsonFileAnalyseService analyseService) 
    : base(services)
    {
        AnalyseService = analyseService;
    }

    public IJsonFileAnalyseService AnalyseService { get; }

    protected override StringPath OnGetProcessPath()
    {
        var ffprobe = Configuration.GetRequiredSection("Processes:Analyse:ffprobe");
        var ffprobePath = ffprobe.Get<string>();
        return StringPath.CreateExists(ffprobePath);
    }

    protected override ProcessSettings OnConfigureSettings()
    {
        var settings = base.OnConfigureSettings();
        settings.Argument = $"-i \"{_analyseFilePath.Value}\" -v quiet -print_format json -show_format -show_streams";
        settings.IsReadOutputs = true;
        return settings;
    }

    public async Task<IFileAnalyse> GetAnalyseAsync(IFile file, CancellationToken cancellationToken = default)
    {
        _analyseFilePath = StringPath.CreateExists(file.Path);
        await StartAsync(cancellationToken);
        var analyseJson = TextProcessResult;
        if (string.IsNullOrWhiteSpace(analyseJson.Text))
        {
            throw new AnalyseProcessException("Failed to read ffprobe analyse");
        }
        return AnalyseService.AnalyseByJson(analyseJson.Text);
    }
}
