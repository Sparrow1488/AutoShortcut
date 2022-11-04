namespace Sparrow.Video.Shortcuts.Extensions;

public static class StringExtensions
{
    public static string ChangeFileExtension(this string filePath, string setExtension)
    {
        string fileDirectoryName = Path.GetDirectoryName(filePath);
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        return Path.Combine(fileDirectoryName, fileName + setExtension);
    }

    public static string MakeEmpty(this string row)
    {
        var span = new Span<char>(row.ToArray().Select(x => ' ').ToArray(), 0, row.Length);
        return span.ToString();
    }
}
