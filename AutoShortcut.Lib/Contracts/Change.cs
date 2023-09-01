namespace AutoShortcut.Lib.Contracts;

public abstract class Change<TMeta>
{
    public required object Source { get; init; }
    public required TMeta Meta { get; init; }
    public DateTime ChangedAt { get; } = DateTime.UtcNow;
}