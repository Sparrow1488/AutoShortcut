namespace Sparrow.Video.Abstractions.Services;

public interface ITextFormatter
{
    string GetPrintable(string veryLongTextIsNotGoodToPrint);
}