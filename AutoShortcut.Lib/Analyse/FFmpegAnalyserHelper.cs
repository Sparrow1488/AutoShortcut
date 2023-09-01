using AutoShortcut.Lib.Analyse.Entries;
using AutoShortcut.Lib.Collections;
using AutoShortcut.Lib.Configuration;
using AutoShortcut.Lib.Contracts;
using AutoShortcut.Lib.Contracts.Enums;
using AutoShortcut.Lib.Contracts.Media;
using AutoShortcut.Lib.Contracts.Services;
using AutoShortcut.Lib.Contracts.Services.Primitives;
using AutoShortcut.Lib.Contracts.Services.Results;
using AutoShortcut.Lib.Exceptions;
using AutoShortcut.Lib.Models;
using IConfigurationProvider = AutoShortcut.Lib.Contracts.Services.IConfigurationProvider;

namespace AutoShortcut.Lib.Analyse;

public class FFmpegAnalyserHelper : IAnalyserHelper
{
    private readonly IProcessManager _diagnostic;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ISerializeService _serializeService;

    public FFmpegAnalyserHelper(
        IProcessManager diagnostic, 
        IConfigurationProvider configurationProvider,
        ISerializeService serializeService)
    {
        _diagnostic = diagnostic;
        _configurationProvider = configurationProvider;
        _serializeService = serializeService;
    }
    
    public async Task AnalyseMediaAsync(IMediaFile media, CancellationToken ctk = default)
    {
        var config = _configurationProvider.GetFFmpegConfig();
        
        var result = await _diagnostic.RunAsync(RunInfo(config, media), ctk);
        if (result is not ProcessStringOutput stringOutput) 
            throw new IncorrectValueException($"Diagnostic output type of {nameof(ProcessOutput)} should be {nameof(ProcessStringOutput)}");
        
        var analyseJson = stringOutput.GetOutput().ToString();
        var data = _serializeService.Deserialize<FFprobeAnalyseData>(analyseJson);
        MapAnalyse(data, media);
    }

    private static ProcessRunInfo RunInfo(FFmpegConfig config, IMediaFile media)
    {
        var info = config.FFprobeRunInfo;
        info.ShowConsole = false;
        info.Arguments = $"-i \"{media.Path}\" -v quiet -print_format json -show_format -show_streams";
        
        return info;
    }

    private static void MapAnalyse(FFprobeAnalyseData source, IAnalysed target)
    {
        var video = source.Streams.FirstOrDefault(x => x.CodecType == CodecType.Video);
        var audio = source.Streams.FirstOrDefault(x => x.CodecType == CodecType.Audio);

        var list = new List<IStreamAnalyse>();

        if (video is not null)
        {
            target.VideoAnalyse = new VideoStreamAnalyse
            {
                Index = video.Index,
                BitRate = video.BitRate,
                CodecName = video.CodecName,
                Duration = video.Duration,
                Height = video.Height,
                Width = video.Width,
                DisplayAspectRatio = video.DisplayAspectRation!
            };
            list.Add(target.VideoAnalyse);
        }

        if (audio is not null)
        {
            target.AudioAnalyse = new AudioStreamAnalyse
            {
                Index = audio.Index,
                BitRate = audio.BitRate,
                CodecName = audio.CodecName,
                Duration = audio.Duration
            };
            list.Add(target.AudioAnalyse);
        }

        if (source.Format is not null)
        {
            target.MediaFormat = new MediaFormat
            {
                Duration = source.Format.Duration,
                FormatName = source.Format.FormatName
            };
        }
            
        target.StreamAnalyses = new StreamCollection(list);
    }
}