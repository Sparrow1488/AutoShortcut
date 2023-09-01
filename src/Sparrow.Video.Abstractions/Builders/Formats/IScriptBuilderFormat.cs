namespace Sparrow.Video.Abstractions.Builders.Formats;

public interface IScriptBuilderFormat
{
    IEnumerable<string> UseFormat(IEnumerable<string> inputBuilderCommand);
}