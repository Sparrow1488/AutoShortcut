using Microsoft.Extensions.Configuration;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class TextFormatter : ITextFormatter
{
    public TextFormatter(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public string GetPrintable(string veryLongTextIsNotGoodToPrint)
    {
        var maxFileNameChars = Configuration
            .GetSection("Output:Text:MaxFileNameChars")
            .Get<int>();
        var goodText = new string(veryLongTextIsNotGoodToPrint.Take(maxFileNameChars).ToArray());
        if (goodText.Length < veryLongTextIsNotGoodToPrint.Length) {
            goodText += "...";
        }
        return goodText;
    }
}