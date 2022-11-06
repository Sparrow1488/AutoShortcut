using Sparrow.Video.Abstractions.Projects.Options;

namespace Sparrow.Video.Abstractions.Services;

public interface IRestoreProjectOptionsService
{
    Task<IProjectOptions> RestoreOptionsAsync();
    Task<IProjectOptions> RestoreOptionsAsync(string optionsFilePath);
}
