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
    }
}
