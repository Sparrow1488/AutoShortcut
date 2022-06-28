using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Processes
{
    public interface IFormatorProcess
    {
        Task<IFile> CreateInFormatAsync(
            IFile toFormat, IFileAnalyse analyse, 
                IVideoFormatSettings settings, ISaveSettings saveSettings);
    }
}
