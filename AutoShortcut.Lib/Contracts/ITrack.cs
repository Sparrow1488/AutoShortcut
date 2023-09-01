using AutoShortcut.Lib.Contracts.Montage;

namespace AutoShortcut.Lib.Contracts;

public interface ITrack
{
    IEnumerable<IRenderMedia> Media { get; }
}