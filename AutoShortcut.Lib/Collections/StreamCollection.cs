using System.Collections;
using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Enums;

namespace AutoShortcut.Lib.Collections;

public class StreamCollection : IEnumerable<IStreamAnalyse>
{
    private readonly List<IStreamAnalyse> _streams;

    public StreamCollection(IEnumerable<IStreamAnalyse> items)
    {
        _streams = new List<IStreamAnalyse>(items);
    }

    public StreamCollection() : this(Enumerable.Empty<IStreamAnalyse>())
    {
        
    }

    public IVideoAnalyse? GetVideo() 
        => _streams.FirstOrDefault(x => x.Kind == StreamAnalyseKind.Video) as IVideoAnalyse;
    
    public IAudioAnalyse? GetAudio() 
        => _streams.FirstOrDefault(x => x.Kind == StreamAnalyseKind.Audio) as IAudioAnalyse;

    public IEnumerator<IStreamAnalyse> GetEnumerator() => _streams.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _streams.GetEnumerator();
}