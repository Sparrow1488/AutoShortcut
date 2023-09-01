namespace AutoShortcut.Lib.Montage.Effects;

public class EffectStoreSettings
{
    public EffectStoreSettings(string? extension)
    {
        Extension = extension;
    }
    
    public string? Extension { get; set; }
}