using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts.Core;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Contracts.Services.Primitives;
using AutoShortcut.Lib.Exceptions;

namespace AutoShortcut.Lib.Core;

public class FFmpegScriptEngine : IFFmpegEngine
{
    private readonly IProcessManager _process;
    private readonly IMediaManager _mediaManager;
    private readonly FFmpegConfig _ffmpegConfig;
    private readonly StorageConfig _storeConfig;
    private string? _savePath;

    public FFmpegScriptEngine(IProcessManager process, IConfigurationProvider config, IMediaManager mediaManager)
    {
        _process = process;
        _mediaManager = mediaManager;
        _ffmpegConfig = config.GetFFmpegConfig();
        _storeConfig = config.GetStorageConfig();
    }
    
    public async Task<MediaScriptResult> ExecuteAsync(Script script, MediaExecutionContext context, CancellationToken ctk = default)
    {
        RequireScript(script);

        var info = RunInfo(_ffmpegConfig, script, context);
        await _process.RunAsync(info, ctk);

        var media = await _mediaManager.LoadAnalysedAsync(GetSaveFilePath(context), ctk);

        ResetProperties();
        return new MediaScriptResult(media, script);
    }

    private static void RequireScript(Script script)
    {
        if (script is FFmpegScript)
            return;
            
        if (script.Content is string)
            return;

        throw new ScriptException($"Passed script content ({script.Content}) or type {script.GetType().Name} incorrect. Current {nameof(FFmpegScriptEngine)} use only scripts of type {nameof(FFmpegScript)} or string content contains ffmpeg command");
    }

    private string GetSaveFilePath(MediaExecutionContext context)
    {
        _savePath ??= context.StoreType == MediaStoreType.Personal
            ? _storeConfig.PersonalFilePath(context.FileName)
            : _storeConfig.TemporaryFilePath(context.FileName);

        return _savePath;
    }

    private void ResetProperties() => _savePath = null;

    private ProcessRunInfo RunInfo(FFmpegConfig config, Script script, MediaExecutionContext context)
    {
        var info = config.FFmpegRunInfo;
        var command = script.Content.ToString()!.Trim();

        if (context.InsertSavePath)
        {
            command += $" \"{GetSaveFilePath(context)}\"";
        }

        info.ShowConsole = true;
        info.Arguments = command;
        return info;
    }
}