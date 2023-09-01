using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Exceptions;

namespace AutoShortcut.Lib.Media;

public class MediaManager : IMediaManager
{
    private readonly IAnalyser _analyser;

    public MediaManager(IAnalyser analyser)
    {
        _analyser = analyser;
    }
    
    public Task<IMediaFile> LoadAsync(string path, CancellationToken ctk = default)
    {
        if (!File.Exists(path))
            throw new UnknownFileException($"Input file '{path}' not exists");

        return Task.FromResult((IMediaFile) new MediaFile(path));
    }

    public async Task<IMediaFile> LoadAnalysedAsync(string path, CancellationToken ctk = default)
    {
        var file = await LoadAsync(path, ctk);
        await _analyser.AnalyseAsync(file, ctk);

        return file;
    }
}