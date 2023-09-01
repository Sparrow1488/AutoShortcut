using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Montage;

namespace AutoShortcut.Lib.Montage;

/// <summary>
/// Дорожка с видео и аудио
/// </summary>
public class MediaTrack : ITrack
{
    private readonly List<IRenderMedia> _media = new();
    
    public MediaTrack(IEnumerable<IMediaFile> files)
    {
        files.ToList().ForEach(x => _media.Add(new RenderMedia(x)));
    }

    public IEnumerable<IRenderMedia> Media => _media.AsReadOnly();
}