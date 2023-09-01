namespace AutoShortcut.Lib.Contracts.Services.Results;

public abstract class ProcessOutput<TOutput> : ProcessOutput
{
    private readonly TOutput _output;

    public ProcessOutput(TOutput output)
    {
        _output = output;
    }

    public override bool TryGetOutput(out object? output)
    {
        output = _output;
        return output is not null;
    }

    public TOutput GetOutput() => _output;
}