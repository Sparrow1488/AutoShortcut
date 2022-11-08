﻿using Sparrow.Video.Shortcuts.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public class SnapshotCommandParameters : CommandParameters
{
    public TimeSpan Time { get; set; }
    public string FromFilePath { get; set; }
    public override string SaveFileName { get; set; }
}
