using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Services;

public interface IProjectSaveSettingsCreator
{
    ISaveSettings Create(string sectionName, string fileName);
}
