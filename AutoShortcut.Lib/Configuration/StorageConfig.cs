namespace AutoShortcut.Lib.Configuration;

public class StorageConfig
{
    public StorageConfig(string temporaryFilesDirectory, string personalFilesDirectory)
    {
        TemporaryFilesDirectory = temporaryFilesDirectory;
        PersonalFilesDirectory = personalFilesDirectory;
    }
    
    public string PersonalFilesDirectory { get; }
    public string TemporaryFilesDirectory { get; }

    public string TemporaryFilePath(string fileName)
    {
        return CombineFixed(TemporaryFilesDirectory, fileName);
    }
    
    public string PersonalFilePath(string fileName)
    {
        return CombineFixed(PersonalFilesDirectory, fileName);
    }

    private static string CombineFixed(string a, string b) => Path.Combine(a, b).Replace("\\", "/");
}