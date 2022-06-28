using Sparrow.Console.Rules;
using Sparrow.Video.Primitives;
using Sparrow.Video.Shortcuts.Factories;

namespace Sparrow.Console
{
    internal class CLI
    {
        public CLI()
        {
            string filesDirectory = @"D:\Йога\SFM\отдельно sfm\55";
            FilesDirectoryPath = StringPath.CreateExists(filesDirectory);
        }

        private StringPath FilesDirectoryPath { get; }

        public async Task OnStart(CancellationToken cancellationToken = default)
        {
            var factory = new ShortcutEngineFactory();
            var engine = factory.CreateEngine();
            var pipeline = await engine.CreatePipelineAsync(
                            FilesDirectoryPath.Value, cancellationToken);

            var project = pipeline.Configure(options =>
            {
                options.IsSerialize = false;
                options.Rules.Add(ApplicationFileRules.FormatFileRule);
                options.Rules.Add(ApplicationFileRules.SilentFileRule);
                options.Rules.Add(ApplicationFileRules.LoopFileRule);
            }).CreateProject();

            var compilation = await engine.StartRenderAsync(project, cancellationToken);
        }
    }
}
