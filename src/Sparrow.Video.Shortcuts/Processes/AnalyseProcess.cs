using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Primitives;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class AnalyseProcess : ExecutionProcessBase, IAnalyseProcess
    {
        public AnalyseProcess(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        protected override StringPath OnGetProcessPath()
        {
            var ffprobe = Configuration.GetRequiredSection("Processes:Analyse:ffprobe");
            var ffprobePath = ffprobe.Get<string>();
            return StringPath.CreateExists(ffprobePath);
        }

        public async Task<IAnalyse> GetAnalyseAsync()
        {
            await StartAsync();
            // получаем json по файлу
            return new Analyse()
            {
                FileType = FileType.Undefined
            };
        }
    }
}
