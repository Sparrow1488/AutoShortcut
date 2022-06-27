namespace Sparrow.Video.Abstractions.Enums
{
    public class RuleName
    {
        private RuleName(string name)
        {
            Value = name;
        }

        public string Value { get; }

        public static readonly RuleName Loop = new RuleName(nameof(Loop));
        public static readonly RuleName Group = new RuleName(nameof(Group));
        public static readonly RuleName Formating = new RuleName(nameof(Formating));
        public static readonly RuleName Encoding = new RuleName(nameof(Encoding));

        public static RuleName New(string name)
        {
            // TODO: проверка входа
            return new RuleName(name);
        }
    }
}
