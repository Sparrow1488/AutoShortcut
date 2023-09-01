using Sparrow.Video.Abstractions.Builders.Formats;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Abstractions.Builders;

public interface IScriptBuilder
{
    IScriptBuilder Insert(string argument);
    IScriptBuilder InsertLast(string argument);
    IScript BuildScript();
    IScript BuildScript(IScriptBuilderFormat format);
    string BuildCommand();
}