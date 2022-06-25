using Sparrow.Video.Abstractions.Primitives;
using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Abstractions.Processes.Settings;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Processes
{
    public class EncodingProcess : ExecutionProcessBase, IEncodingProcess
    {
        protected override StringPath OnGetProcessPath()
        {
            throw new NotImplementedException();
        }

        public Task<IFile> StartEncodingAsync(IFile encodable, IEncodingSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
