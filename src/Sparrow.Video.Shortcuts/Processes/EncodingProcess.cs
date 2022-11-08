using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Processes.Abstractions;

namespace Sparrow.Video.Shortcuts.Processes;

public class EncodingProcess : FFmpegProcess, IEncodingProcess // 1. МОЖЕТ ЭТО УБРАТЬ
{
    private StringPath _filePath;
    private IEncodingSettings _settings;
    private ISaveSettings _saveSettings;

    public EncodingProcess(IServiceProvider services)
    : base(services)
    {
    }

    protected override string OnConfigureFFmpegCommand() =>
        $"-i \"{_filePath.Value}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -crf 17 -f {_settings.EncodingType} \"{_saveSettings.SaveFullPath}\" ";
    protected override ISaveSettings OnConfigureSaveSettings() => _saveSettings;
    // 2. ЭТОТ МЕТОД УБРАТЬ И ОСТАВИТЬ ТОЛЬКО FFMPEG COMMAND
    public async Task<IFile> StartEncodingAsync(
        IFile encodable, IEncodingSettings settings, ISaveSettings saveSettings, CancellationToken cancellationToken = default)
    {
        _settings = settings;
        _saveSettings = saveSettings;
        _filePath = StringPath.CreateExists(encodable.Path);
        return await StartFFmpegAsync(cancellationToken);
    }
}
