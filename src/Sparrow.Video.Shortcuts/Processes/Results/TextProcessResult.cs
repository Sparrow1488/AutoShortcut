using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Results;

public class TextProcessResult : ProcessResult, ITextProcessResult
{
    public TextProcessResult(ProcessSettings settings, string text) : base(settings)
    {
        Text = text;
    }

    public string Text { get; }
}
