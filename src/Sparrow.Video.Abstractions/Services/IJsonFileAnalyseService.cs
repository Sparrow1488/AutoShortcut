using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services;

public interface IJsonFileAnalyseService : IFileAnalyseService
{
    IFileAnalyse AnalyseByJson(string jsonAnalyse);
}