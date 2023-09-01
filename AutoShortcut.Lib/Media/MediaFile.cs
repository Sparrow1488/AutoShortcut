using AutoShortcut.Lib.Collections;
using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Media;

public class MediaFile : IMediaFile
{
    public MediaFile(string path)
    {
        Path = path;
        StreamAnalyses = new StreamCollection();
    }

    public string Path { get; }
    public string FullPath => System.IO.Path.GetFullPath(Path);
    public string Name => System.IO.Path.GetFileName(Path);
    public string Extension => System.IO.Path.GetExtension(Name);
    public IVideoAnalyse? VideoAnalyse { get; set; }
    public IAudioAnalyse? AudioAnalyse { get; set; }
    public IMediaFormat? MediaFormat { get; set; }
    public StreamCollection StreamAnalyses { get; set; }
}