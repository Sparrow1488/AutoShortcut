using Sparrow.Video.Abstractions.Services.Options;

namespace Sparrow.Video.Abstractions.Services
{
    public interface ISaveService
    {
        Task SaveAsync<TObject>(
            TObject @object, ISaveOptions saveOptions, CancellationToken cancellationToken = default);
    }
}
