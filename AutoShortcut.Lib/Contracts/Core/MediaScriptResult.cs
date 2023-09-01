using AutoShortcut.Lib.Contracts.Media;

namespace AutoShortcut.Lib.Contracts.Core;

public record MediaScriptResult(
    IMediaFile Media,
    Script Script
) : ScriptExecutionResult(Script);