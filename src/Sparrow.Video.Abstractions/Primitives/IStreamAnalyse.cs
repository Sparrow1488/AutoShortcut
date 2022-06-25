namespace Sparrow.Video.Abstractions.Primitives
{
    public interface IStreamAnalyse
    {
        public int Index { get; }
        public string CodecName { get; }
        public string CodecType { get; }
        public double Duration { get; }
        public int BitRate { get; set; }
    }
}
