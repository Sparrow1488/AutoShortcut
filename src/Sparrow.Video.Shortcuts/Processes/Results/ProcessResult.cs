using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Results
{
    public class ProcessResult : IProcessResult
    {
        public ProcessResult(ProcessSettings settings, string text)
        {
            Settings = settings;
            Text = text;
        }

        public ProcessSettings Settings { get; }
        public string Text { get; }
    }
}
