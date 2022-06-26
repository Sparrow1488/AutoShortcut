namespace Sparrow.Video.Abstractions.Services
{
    public interface IStoreService
    {
        Task<TObject> GetObjectAsync<TObject>(string name);
    }
}
