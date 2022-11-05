using Newtonsoft.Json;
using Sparrow.Video.Abstractions.Services;

namespace Sparrow.Video.Shortcuts.Services;

public class JsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
        CheckAdditionalContent = true,
        TypeNameHandling = TypeNameHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Error,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
    };

    public TObj Deserialize<TObj>(string json)
    {
        return JsonConvert.DeserializeObject<TObj>(json, _jsonSettings);
    }

    public string Serialize<TObj>(TObj obj)
    {
        return JsonConvert.SerializeObject(obj, _jsonSettings);
    }
}
