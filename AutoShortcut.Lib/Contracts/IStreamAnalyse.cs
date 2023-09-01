using AutoShortcut.Lib.Contracts.Enums;

namespace AutoShortcut.Lib.Contracts;

public interface IStreamAnalyse
{
    int Index { get; }
    string CodecName { get; }
    double Duration { get; }
    int BitRate { get; }
    StreamAnalyseKind Kind { get; }
}