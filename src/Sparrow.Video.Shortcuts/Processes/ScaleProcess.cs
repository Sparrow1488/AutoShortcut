using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Shortcuts.Processes.Abstractions;

namespace Sparrow.Video.Shortcuts.Processes;

public class ScaleProcess : FFmpegProcess, IScaleProcess
{
    public ScaleProcess(IServiceProvider services)
    : base(services)
    {
    }

    private ISaveSettings _saveSettings;
    private IScaleSettings _scaleSettings;
    private IFile _file;

    public async Task<IFile> ScaleVideoAsync(
        IFile file, 
        IScaleSettings scaleSettings, 
        ISaveSettings saveSettings, 
        CancellationToken cancellationToken = default)
    {
        _saveSettings = saveSettings;
        _scaleSettings = scaleSettings;
        _file = file;
        return await StartFFmpegAsync(cancellationToken);
    }

    /// <summary>
    ///     StackOverflow solution: https://stackoverflow.com/questions/34391499/change-video-resolution-ffmpeg
    /// </summary>
    protected override string OnConfigureFFmpegCommand() =>
        $"-i \"{_file.Path}\" -vf \"[in] scale=iw* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Height}/ih):ih* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Height}/ih)[scaled]; [scaled] pad={_scaleSettings.Width}:{_scaleSettings.Height}:({_scaleSettings.Width}-iw* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Height}/ih))/2:({_scaleSettings.Height}-ih* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Height}/ih))/2[padded]; [padded] setsar=1:1[out]\" -c:v libx264 -c:a copy \"{_saveSettings.SaveFullPath}\"";

    protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;
}
