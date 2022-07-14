namespace Sparrow.Video.Abstractions.Services
{
    public interface IJsonSerializer
    {
        string Serialize<TObj>(TObj obj);
        TObj Deserialize<TObj>(string json);
    }
}
