namespace Sparrow.Video.Abstractions.Enums;

public class FrameFrequency
{
    public FrameFrequency(int fps)
    {
        Value = fps;
    }

    public int Value { get; }

    public static readonly FrameFrequency Fps120 = new FrameFrequency(120);
    public static readonly FrameFrequency Fps90 = new FrameFrequency(90);
    public static readonly FrameFrequency Fps60 = new FrameFrequency(60);
    public static readonly FrameFrequency Fps50 = new FrameFrequency(50);
    public static readonly FrameFrequency Fps30 = new FrameFrequency(30);
    public static readonly FrameFrequency Fps25 = new FrameFrequency(25);
}