namespace AutoShortcut.Lib.Contracts.Services;

public interface ISerializeService
{
    string Serialize<T>(T value);
    T Deserialize<T>(string json);
}
