namespace AutoShortcut.Lib.Contracts.Services.Results;

public abstract class ProcessOutput
{
    public abstract bool TryGetOutput(out object? output);
}