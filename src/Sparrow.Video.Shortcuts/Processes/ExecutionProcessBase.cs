using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Primitives;

namespace Sparrow.Video.Shortcuts.Processes
{
    public abstract class ExecutionProcessBase : IExecutionProcess
    {
        protected abstract StringPath OnGetProcessPath();

        public Task StartAsync()
        {
            var path = OnGetProcessPath();
            return Task.CompletedTask;
        }
    }
}
