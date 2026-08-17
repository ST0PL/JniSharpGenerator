using System.Text.RegularExpressions;

namespace JniSharpGenerator.Parser
{
    public partial class JavaMethod : JavaEntity
    {
        public string[] Throws { get; }
        public string[] Arguments { get; }
        public string ReturnType { get; }
        public bool IsConstructor { get; }
        public bool IsAbstract { get; }
        public bool IsNative { get; set; }

        private JavaMethod(AccessModifier accessModifier, string name, string[] throws, string[] arguments, string returnType, bool isAbstract, bool isNative, bool isFinal, bool isStatic, bool isConstructor) : base(accessModifier, name, isFinal, isStatic)
        {
            Throws = throws;
            Arguments = arguments;
            ReturnType = returnType;
            IsAbstract = isAbstract;
            IsConstructor = isConstructor;
            IsNative = isNative;
        }

        public static JavaMethod? Parse(string methodString)
        {
            if (string.IsNullOrWhiteSpace(methodString) || !methodString.IsMatch(Expressions.MethodSignature))
                return null;

            var groups = Regex.Match(methodString, Expressions.MethodSignature).Groups;
            var modifiers = groups["modifiers"].Value.Split();

            AccessModifier accessModifier = modifiers.FirstOrDefault()?.ToEnumOrDefault<AccessModifier>(true) ?? default;

            string returnType = groups["returnType"].Value;
            bool isStatic = modifiers.Contains("static");
            bool isFinal = modifiers.Contains("final");
            bool isAbstract = !isFinal && modifiers.Contains("abstract");
            bool isConstructor = !groups["returnType"].Success;
            bool isNative = modifiers.Contains("native");

            returnType = isConstructor ? "void" : returnType;

            string name = groups["name"].Value.Split(".")[^1];
            string argumentsString = groups["args"].Value;
            string[] arguments = argumentsString.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            string throwsString = groups["throws"].Value;
            string[] throws = throwsString.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            return new(accessModifier, name, throws, arguments, returnType, isAbstract, isNative, isFinal, isStatic, isConstructor);
        }
    }
}