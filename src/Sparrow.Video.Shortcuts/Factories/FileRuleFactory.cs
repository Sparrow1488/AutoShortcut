using Sparrow.Video.Abstractions.Rules;

namespace Sparrow.Video.Shortcuts.Factories;

public abstract class FileRuleFactory
{
    /// <summary>
    ///     Creates any <see cref="IFileRule"/> without constructor!!!
    /// </summary>
    public static T CreateDefaultRule<T>()
        where T : IFileRule
    {
        return (T)Activator.CreateInstance(typeof(T));
    }

    /// <summary>
    ///     Creates any <see cref="IFileRule"/> without constructor!!!
    /// </summary>
    public static IFileRule CreateDefaultRule(Type type)
    {
        return (IFileRule)Activator.CreateInstance(type);
    }
}
