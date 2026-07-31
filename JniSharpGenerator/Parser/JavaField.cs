using System.Text.RegularExpressions;

namespace JniSharpGenerator.Parser
{
    internal class JavaField : JavaEntity
    {
        public string Type { get; }

        private JavaField(AccessModifier accessModifier, string name, string type, bool isFinal, bool isStatic) : base(accessModifier, name, isFinal, isStatic)
        {
            Type = type;
        }

        public static JavaField? Parse(string fieldString)
        {
            if (string.IsNullOrWhiteSpace(fieldString) || !fieldString.IsMatch(Expressions.FieldSignature))
                return null;

            var groups = Regex.Match(fieldString, Expressions.FieldSignature).Groups;
            var modifiers = groups["modifiers"].Value.Split();

            AccessModifier accesssModifier = modifiers.FirstOrDefault()?.ToEnumOrDefault<AccessModifier>(true) ?? default;
            string name = groups["name"].Value;
            string type = groups["type"].Value;
            bool isFinal = modifiers.Contains("final");
            bool isStatic = modifiers.Contains("static");

            return new(accesssModifier, name, type, isFinal, isStatic);
        }
    }
}