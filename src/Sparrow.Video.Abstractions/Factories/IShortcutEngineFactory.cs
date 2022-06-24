using Sparrow.Video.Abstractions.Enginies;

namespace Sparrow.Video.Abstractions.Factories
{
    /// <summary>
    ///     Создает DI и затягивает все зависимости
    /// </summary>
    public interface IShortcutEngineFactory
    {
        IShortcutEngine CreateEngine();
    }
}
