namespace Sparrow.Video.Abstractions.Primitives
{
    public interface IVideoStreamAnalyse : IStreamAnalyse
    {
        public int Width { get; }
        public int Height { get; }
        public string DisplayAspectRatio { get; }
    }
}
