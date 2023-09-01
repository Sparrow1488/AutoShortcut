using AutoShortcut.Lib.Contracts.Services;
using Newtonsoft.Json;

namespace AutoShortcut.Lib.Helpful;

public class SerializeService : ISerializeService
{
    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, Formatting.Indented);
    }

    public T Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json) ?? throw new JsonSerializationException($"Failed to deserialize json to type {nameof(T)}");
    }
}