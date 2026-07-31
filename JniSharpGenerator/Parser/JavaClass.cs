using System.Text.RegularExpressions;

namespace JniSharpGenerator.Parser
{
    internal class JavaClass : JavaEntity
    {
        public string Package { get; }
        public IReadOnlyCollection<string> Annotations { get; }
        public IReadOnlyList<JavaField?> Fields { get; }
        public IReadOnlyList<JavaMethod?> Methods { get; }
        public string Generics { get; }
        public IReadOnlyList<string> Implements { get; }
        public IReadOnlyList<string> Extends { get; }
        public bool IsAbstract { get; }
        public bool IsSealed { get; }

        private JavaClass(string package, IReadOnlyList<string> annotations, AccessModifier accessModifier, string name, IReadOnlyList<JavaField?> fields, IReadOnlyList<JavaMethod?> methods, string generics, IReadOnlyList<string> implements, IReadOnlyList<string> extends, bool isFinal, bool isAbstract, bool isSealed, bool isStatic) : base(accessModifier, name, isFinal, isStatic)
        {
            Package = package;
            Annotations = annotations;
            Fields = fields;
            Methods = methods;
            Generics = generics;
            Implements = implements;
            Extends = extends;
            IsAbstract = isAbstract;
            IsSealed = isSealed;
        }

        public static JavaClass? Parse(string klassString)
        {
            if(string.IsNullOrWhiteSpace(klassString) || !klassString.IsMatch(Expressions.ClassSignature))
                return null;

            var classDefinition = Regex.Match(klassString, Expressions.ClassSignature, RegexOptions.Multiline).Groups;
            var modifiers = classDefinition["modifiers"].Value.Split();

            AccessModifier accessModifier = modifiers.FirstOrDefault()?.ToEnumOrDefault<AccessModifier>(true) ?? default;

            bool isFinal = modifiers.Contains("final");
            bool isStatic = modifiers.Contains("static");
            bool isAbstract = !isFinal && modifiers.Contains("abstract");
            bool isSealed = modifiers.Contains("sealed");

            string[] annotations = classDefinition["annotations"].Value.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string generics = classDefinition["generics"].Value;
            string[] implements = classDefinition["implements"].Value.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            string[] extends = classDefinition["extends"].Value.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            string[] nameParts = classDefinition["name"].Value.Split(".");
            string package = string.Join(".", nameParts[0..^1]);
            string name = nameParts[^1];

            var fields = Regex.Matches(klassString, Expressions.FieldSignature, RegexOptions.Multiline).Select(f => JavaField.Parse(f.Value)).ToList();
            var methods = Regex.Matches(klassString, Expressions.MethodSignature, RegexOptions.Multiline).Select(m => JavaMethod.Parse(m.Value)).ToList();

            return new(package, annotations.AsReadOnly(), accessModifier, name, fields.AsReadOnly(), methods.AsReadOnly(), generics, implements.AsReadOnly(), extends.AsReadOnly(), isFinal, isAbstract, isSealed, isStatic);
        }
    }
}