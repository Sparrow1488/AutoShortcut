using AutoShortcut.Lib.Contracts.Core;

namespace AutoShortcut.Lib.Core;

public record FFmpegScript(string Command) : Script(Command);