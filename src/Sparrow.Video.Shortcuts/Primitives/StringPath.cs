using Sparrow.Video.Shortcuts.Exceptions;

namespace Sparrow.Video.Primitives
{
    public struct StringPath
    {
        private StringPath(string path)
        {
            Value = path ?? "";
        }

        public string Value { get; }

        /// <summary>
        ///     Create required path or throw <see cref="InvalidInputPathException"/>
        /// </summary>
        /// <returns>Required path</returns>
        /// <exception cref="InvalidInputPathException"></exception>
        public static StringPath Create(string path)
        {
            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                var fullPath = Path.GetFullPath(path);
                return new StringPath(fullPath);
            }
            throw new InvalidInputPathException($"Invalid path => {path ?? ""}");
        }

        public override string ToString() => Value;
    }
}
