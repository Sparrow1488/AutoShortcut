using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Services;

public interface IFileAnalyseService
{
    IFileAnalyse Analyse(string variables);
}