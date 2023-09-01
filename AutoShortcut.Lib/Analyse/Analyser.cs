using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Services;

namespace AutoShortcut.Lib.Analyse;

public class Analyser : IAnalyser
{
    private readonly IAnalyserHelper _helper;

    public Analyser(IAnalyserHelper helper)
    {
        _helper = helper;
    }
    
    public Task AnalyseAsync(IMediaFile file, CancellationToken ctk = default)
    {
        return _helper.AnalyseMediaAsync(file, ctk);
    }
}