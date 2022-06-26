using Microsoft.Extensions.Configuration;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Processes
{
    public abstract class FFmpegProcess : ExecutionProcessBase
    {
        public FFmpegProcess(IConfiguration configuration) : base(configuration)
        {
        }

        protected override StringPath OnGetProcessPath()
        {
            var ffmpegPath = Configuration.GetRequiredSection("Processes:Video:ffmpeg")
                                          .Get<string>();
            return StringPath.CreateExists(ffmpegPath);
        }
    }
}
