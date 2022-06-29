using Sparrow.Video.Abstractions.Projects;

namespace Sparrow.Video.Abstractions.Pipelines
{
    /// <summary>
    ///     This is necessary to set up rules for <see cref="IProjectFile"/>'s, 
    ///     as well as to create a <see cref="IProject"/>
    /// </summary>
    public interface IShortcutPipeline : IPipeline
    {
    }
}
