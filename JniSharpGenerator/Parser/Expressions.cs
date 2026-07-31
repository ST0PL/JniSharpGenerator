namespace JniSharpGenerator.Parser
{
    public static class Expressions
    {
        public const string TypeSignature =
            @"^\s*(?<name>[\w$]+(?:\.[\w$]+)*)(?:\s*(?<generics><\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>)\s*)?(?<dimensions>(?<vararg>\.\.\.)|(?:\s*\[\s*\])+)*";
        public const string ClassSignature =
            @"\s*(?<annotations>(?:@[\w.$]+\s+)*)(?<modifiers>(?:(?:public|private|protected|static|abstract|final|sealed|non-sealed|strictfp)\s+)*)class\s+(?<name>[\w.$]+)(?:\s*<\s*(?<classGenerics>(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>))?(?:\s+extends\s+(?<extends>[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>)?))?(?:\s+implements\s+(?<implements>(?:[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>)?)(?:\s*,\s*[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>)?)*))?(?:\s+permits\s+(?<permits>(?:[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>)?)(?:\s*,\s*[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>)?)*))?\s*\{(?<content>[\s\S]*?\s)\}";
        public const string FieldSignature =
            @"^\s*(?<modifiers>(?:(?:public|private|protected|static|final|volatile)\s+)*)\s?(?<type>[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>\s*)?(?:\s*\[\s*\])*)\s+(?<name>[\w.$]+);";
        public const string MethodSignature =
            @"^\s*(?<modifiers>(?:(?:public|private|protected|static|abstract|final|native|synchronized|strictfp)\s+)*)(?:(?<returnType>[\w.$]+(?:\s*<\s*(?:[^<>]|(?<open><)|(?<close-open>>))*\s*(?(open)(?!))\s*>\s*)?(?:\s*\[\s*\])*)\s+(?=[a-zA-Z0-9_$]+\s*\())?(?<name>[a-zA-Z0-9_$]+(?:\.[a-zA-Z0-9_$]+)*)\s*\((?<args>[^)]*)\)(?:\sthrows\s(?<throws>[^;]+))?;";
    }
}