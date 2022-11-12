using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Enginies
{
    public interface IFileAnalysisEngine
    {
        Task<IFileAnalyse> CreateAnalyseAsync(IFile file);
    }
}
