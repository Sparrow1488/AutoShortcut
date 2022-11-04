﻿namespace Sparrow.Video.Shortcuts.Enums;

public class EnvironmentVariableNames
{
    /// <summary>
    ///     Specifies that the files used in the project should be serialized for further restoration
    /// </summary>
    public const string Serialize = "serialize";
    /// <summary>
    ///     Specifies the name of output files
    /// </summary>
    public const string OutputName = "output";
    /// <summary>
    ///     Specifies the current project mode (create new or restore exists)
    /// </summary>
    public const string ProjectOpenMode = "mode";
    /// <summary>
    ///     Development or Production (dev or prod)
    /// </summary>
    public const string Environment = "env";
}