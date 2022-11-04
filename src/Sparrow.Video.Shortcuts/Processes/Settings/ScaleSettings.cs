﻿using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Shortcuts.Processes.Settings;

[Serializable]
public class ScaleSettings : IScaleSettings
{
    [JsonConstructor]
    public ScaleSettings() { }

    [JsonProperty]
    public int Width { get; set; } = -2;
    [JsonProperty]
    public int Heigth { get; set; } = -2;

    public static IScaleSettings Create(Resolution resolution)
    {
        return new ScaleSettings()
        {
            Heigth = resolution.Height,
            Width = resolution.Width
        };
    }
}
