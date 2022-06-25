using Sparrow.Video.Abstractions.Processes;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Processes.Results;
using Sparrow.Video.Shortcuts.Processes.Settings;
using System.Diagnostics;

namespace Sparrow.Video.Shortcuts.Processes
{
    public abstract class ExecutionProcessBase : IExecutionProcess
    {
        protected ITextProcessResult TextProcessResult { get; private set; }

        public async Task StartAsync()
        {
            string result = "";
            var settings = OnConfigureSettings();
            var startInfo = CreateStartInfo();

            startInfo.Arguments = settings.Argument;
            using (var process = new Process() { StartInfo = startInfo })
            {
                if (settings.IsReadOutputs)
                {
                    result = await StartReadingAsync(process);
                }
                else
                {
                    process.Start();
                }
                await process.WaitForExitAsync();
            }
            TextProcessResult = new TextProcessResult(settings, result);
        }

        protected abstract StringPath OnGetProcessPath();
        protected virtual ProcessSettings OnConfigureSettings() =>
            new ProcessSettings();

        private ProcessStartInfo CreateStartInfo()
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = OnGetProcessPath().Value,
                WorkingDirectory = Directory.GetCurrentDirectory(),
            };
            var settings = OnConfigureSettings();
            if (settings.IsReadOutputs)
            {
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardInput = true;
                startInfo.UseShellExecute = false;
            }
            if (settings.IsShowConsole)
            {
                startInfo.CreateNoWindow = true;
            }
            return startInfo;
        }

        private async Task<string> StartReadingAsync(Process process)
        {
            process.EnableRaisingEvents = true;
            process.Start();
            return await process.StandardOutput.ReadToEndAsync();
        }

        //private ProcessStartInfo CreateStartInfo()
        //{
        //    var startInfo = CreateStartInfo();
            
        //    startInfo.UseShellExecute = false;
        //    startInfo.CreateNoWindow = true;
        //    return startInfo;
        //}
    }
}
