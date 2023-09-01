using Sparrow.Video.Abstractions.Builders;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Builders;

public class FileRuleBuilder : IFileRuleBuilder
{
    private readonly IFile _file;

    public FileRuleBuilder(IFile file)
    {
        _file = file;
    }

    public IFileRule Build()
    {
        throw new NotImplementedException();
    }

    public IFileRule CreateByCondition(Func<IFile, IFileRule> condition)
    {
        return condition.Invoke(_file);
    }
}