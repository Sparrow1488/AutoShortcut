namespace AutoShortcut.Lib.Contracts;

public interface IVideoAnalyse : IStreamAnalyse
{
    int Width { get; }
    int Height { get; }
    string DisplayAspectRatio { get; }
}