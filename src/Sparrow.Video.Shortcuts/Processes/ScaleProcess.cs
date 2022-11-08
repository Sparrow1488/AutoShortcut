using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;

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
        ThrowIfScaleSettingsInvalid();
        return await StartFFmpegAsync(cancellationToken);
    }

    protected void ThrowIfScaleSettingsInvalid()
    {
        if (_scaleSettings.Heigth < 1 || _scaleSettings.Width < 1)
        {
            throw new InvalidOperationException("" +
                $"Failed to process scale video with input parameters: Height:{_scaleSettings.Heigth}, Width:{_scaleSettings.Width}");
        }
    }

    /// <summary>
    ///     StackOverflow solution: https://stackoverflow.com/questions/34391499/change-video-resolution-ffmpeg
    /// </summary>
    protected override string OnConfigureFFmpegCommand() =>
        $"-i \"{_file.Path}\" -vf \"[in] scale=iw* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Heigth}/ih):ih* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Heigth}/ih)[scaled]; [scaled] pad={_scaleSettings.Width}:{_scaleSettings.Heigth}:({_scaleSettings.Width}-iw* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Heigth}/ih))/2:({_scaleSettings.Heigth}-ih* min({_scaleSettings.Width}/iw\\,{_scaleSettings.Heigth}/ih))/2[padded]; [padded] setsar=1:1[out]\" -c:v libx264 -c:a copy \"{_saveSettings.SaveFullPath}\"";

    protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;
}
