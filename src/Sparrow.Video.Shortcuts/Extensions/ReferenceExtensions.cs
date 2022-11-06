using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Extensions;

public static class ReferenceExtensions
{
    public static IReference GetActual(this IEnumerable<IReference> references)
    {
        var lastReference = references.Where(x => x.Type.Value != ReferenceType.Ignore.Value)
                                      .OrderByDescending(x => x.CreatedAt).First();
        return lastReference;
    }
}
