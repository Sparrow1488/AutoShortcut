using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes.Settings;

namespace Sparrow.Video.Abstractions.Processes
{
    public interface IEncodingProcess : IExecutionProcess
    {
        /// <summary>
        ///     Reencode file to <see cref="EncodingType"/>
        /// </summary>
        /// <param name="encodable">Convertable file</param>
        /// <param name="settings">Configure process</param>
        /// <returns>Reencoded file</returns>
        Task<IFile> StartEncodingAsync(
            IFile encodable, IEncodingSettings settings, ISaveSettings saveSettings);
    }
}
