using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Enums;

namespace AutoShortcut.Lib.Analyse.Entries;

public class AudioStreamAnalyse : StreamAnalyse, IAudioAnalyse
{
    public override StreamAnalyseKind Kind => StreamAnalyseKind.Audio;
}