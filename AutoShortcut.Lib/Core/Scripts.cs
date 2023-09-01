using AutoShortcut.Lib.Helpful;

namespace AutoShortcut.Lib.Core;

public record OpacityScript(
    double Opacity
) : FFmpegScript("TODO");

public record FadeScript(
    string Input,
    int StartSecond,
    int Duration,
    string Suffix
) : FFmpegScript($"-y -i \"{Input}\" -filter_complex fade={Suffix}:st={StartSecond}:d={Duration}");

public record SilentAudioScript(
    string Input
) : FFmpegScript($"-y -f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i \"{Input}\" -c:v copy -c:a aac -shortest");

public record ScaleVideoScript(
    string Input,
    int Width,
    int Height
) : FFmpegScript($"-y -i \"{Input}\" -vf \"[in] scale=iw* min({Width}/iw\\,{Height}/ih):ih* min({Width}/iw\\,{Height}/ih)[scaled]; [scaled] pad={Width}:{Height}:({Width}-iw* min({Width}/iw\\,{Height}/ih))/2:({Height}-ih* min({Width}/iw\\,{Height}/ih))/2[padded]; [padded] setsar=1:1[out]\" -c:v libx264 -preset {Presets.PresetValue} -crf {Presets.CrfValue}");

public record EncodingVideoScript(
    string Input,
    string Encoding
) : FFmpegScript($"-y -i \"{Input}\" -acodec copy -vcodec copy -vbsf h264_mp4toannexb -crf {Presets.CrfValue} -f {Encoding}");

public record LoopVideoDemuxerScript(
    string InputTextPath
) : FFmpegScript($"-y -f concat -safe 0 -i \"{InputTextPath}\" -c copy");

/// <summary>
/// Склеить видео без перекодирования. Могут возникнуть проблемы с timestamp видео из-за чего произойдет рассинхронизация.
/// </summary>
/// <param name="InputFile"></param>
public record MultiplexCombineMediaScript(
    string InputFile
) : FFmpegScript($"-y -f concat -safe 0 -i \"{InputFile}\" -c copy -preset slower -qp 0");

/// <summary>
/// Склеить видео с перекодированием. Отличается от <see cref="MultiplexCombineMediaScript"/> тем, что исправлены проблемы с timestamp, однако видео все так же заедает
/// https://stackoverflow.com/questions/53021266/non-monotonous-dts-in-output-stream-previous-current-changing-to-this-may-result
/// </summary>
/// <param name="InputFile"></param>
public record MultiplexTimestampCombineMediaScript(
    string InputFile
) : FFmpegScript($"-y -safe 0 -f concat -segment_time_metadata 1 -i \"{InputFile}\" -vf select=concatdec_select -af aselect=concatdec_select,aresample=async=1");

/// <summary>
/// Склеить видео с перекодировкой.
/// Больше информации о параметрах -vsync тут: https://www.reddit.com/r/ffmpeg/comments/pvtd1o/vsync_0_vs_vsync_2_for_getting_the_timestamps_of/ 
/// </summary>
/// <param name="InputFile"></param>
public record DemuxerCombineMediaScript(
    string[] Inputs
) : FFmpegScript($"-y {string.Join(" ", Inputs.Select(x => "-i " + x))} -filter_complex \"{string.Join(" ", Enumerable.Range(0, Inputs.Length).Select(x => $"[{x}:v] [{x}:a]"))} concat=n={Inputs.Length}:v=1:a=1 [v] [a]\" -map \"[v]\" -map \"[a]\" -vsync vfr");

public record FpsVideoScript(
    string Input,
    int Fps
) : FFmpegScript($"-y -i \"{Input}\" -filter:v fps=fps={Fps}");