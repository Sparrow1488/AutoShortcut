namespace Sparrow.Video.Shortcuts.Processes.Sources.Parameters;

public class EncodingCommandParameters : CommandParameters
{
    public EncodingCommandParameters(string saveFileName, string fromFilePath) 
    : base(saveFileName, fromFilePath)
    {
    }

    /// <summary>
    ///     One of <see cref="Enums.EncodingType"/>
    /// </summary>
    public string EncodingType { get; set; }
}
