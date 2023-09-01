namespace AutoShortcut.Lib.Media;

public record Encoding(string EncodingFormat, string? ExtensionName = null);
public record MpegTsEncoding() : Encoding("mpegts", ".ts");