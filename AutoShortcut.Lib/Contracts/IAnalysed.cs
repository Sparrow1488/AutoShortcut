using AutoShortcut.Lib.Collections;

namespace AutoShortcut.Lib.Contracts;

public interface IAnalysed
{
    IVideoAnalyse? VideoAnalyse { get; set; }
    IAudioAnalyse? AudioAnalyse { get; set; }
    IMediaFormat? MediaFormat { get; set; }
    StreamCollection StreamAnalyses { get; set; }
}