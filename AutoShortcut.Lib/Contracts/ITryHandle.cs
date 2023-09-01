namespace AutoShortcut.Lib.Contracts;

public interface ITryHandle
{
    bool CanHandle(object toHandle);
}