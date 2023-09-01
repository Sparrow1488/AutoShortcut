namespace AutoShortcut.Lib.Contracts;

public interface IMediaFormat
{
    double Duration { get; }
    /// <summary>
    /// Перечисление форматов через запятую (прим.: mov,mp4,m4a,3gp,3g2,mj2)
    /// </summary>
    string? FormatName { get; }
}