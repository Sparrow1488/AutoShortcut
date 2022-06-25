using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Services;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Exceptions;
using Sparrow.Video.Shortcuts.Extensions;
using Sparrow.Video.Shortcuts.Primitives;
using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class AnalyseProcess : ExecutionProcessBase, IAnalyseProcess
    {
        public AnalyseProcess(
            IConfiguration configuration,
            IJsonFileAnalyseService analyseService)
        {
            Configuration = configuration;
            AnalyseService = analyseService;
        }

        private StringPath _analyseFilePath;

        public IConfiguration Configuration { get; }
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
            if (Configuration.IsDebug())
            {
                settings.IsShowConsole = true;
            }
            return settings;
        }

        public async Task<IFileAnalyse> GetAnalyseAsync(IFile file)
        {
            _analyseFilePath = StringPath.CreateExists(file.Path);
            await StartAsync();
            var analyseJson = TextProcessResult;
            if (string.IsNullOrWhiteSpace(analyseJson.Text))
            {
                throw new AnalyseProcessException("Failed to read ffprobe analyse");
            }
            return AnalyseService.AnalyseByJson(analyseJson.Text);
        }
    }
}
