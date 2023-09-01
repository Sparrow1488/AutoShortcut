namespace AutoShortcut.Lib.Contracts;

public interface IChangesHistory<TChangeMeta>
{
    Change<TChangeMeta>? LastChange { get; }
    IEnumerable<Change<TChangeMeta>> Changes { get; }
}