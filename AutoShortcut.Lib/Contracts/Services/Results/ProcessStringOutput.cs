using System.Text;

namespace AutoShortcut.Lib.Contracts.Services.Results;

public class ProcessStringOutput : ProcessOutput<StringBuilder>
{
    public ProcessStringOutput(StringBuilder output) : base(output)
    {
    }
}