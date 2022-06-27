using Sparrow.Video.Abstractions.Enums;
using Sparrow.Video.Abstractions.Primitives;

namespace Sparrow.Video.Shortcuts.Primitives
{
    public class Reference : IReference
    {
        public string Name { get; set; }
        public string FileFullPath { get; set; }
        public string Target { get; set; }
        public ReferenceType Type { get; set; }
    }
}
