namespace AutoShortcut.Lib.Configuration;

public class ProjectConfig
{
    public ProjectConfig(string? outputFileName = null)
    {
        OutputFileName = outputFileName;
    }
    
    public string? OutputFileName { get; set; }
}