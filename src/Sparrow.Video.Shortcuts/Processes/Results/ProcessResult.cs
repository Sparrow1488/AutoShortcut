using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Results
{
    public class ProcessResult : IProcessResult
    {
        public ProcessResult(ProcessSettings settings)
        {
            Settings = settings;
        }

        public ProcessSettings Settings { get; }
    }
}
