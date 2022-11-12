using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Extensions
{
    public static class ReferenceExtensions
    {
        public static IReference GetActual(this IEnumerable<IReference> references)
        {
            var lastReference = references.OrderByDescending(x => x.CreatedAt).First();
            return lastReference;
        }
    }
}
