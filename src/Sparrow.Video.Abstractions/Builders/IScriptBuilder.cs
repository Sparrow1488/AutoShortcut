using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Builders
{
    public interface IScriptBuilder
    {
        IScriptBuilder Insert(string argument);
        IScriptBuilder InsertLast(string argument);
        IScript BuildScript();
        string BuildCommand();
    }
}
