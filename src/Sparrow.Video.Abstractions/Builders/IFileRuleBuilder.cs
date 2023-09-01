using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Abstractions.Builders;

public interface IFileRuleBuilder
{
    IFileRule CreateByCondition(Func<IFile, IFileRule> condition);
    IFileRule Build();
}