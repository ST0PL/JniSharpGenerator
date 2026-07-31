namespace JniSharpGenerator.Parser
{
    internal abstract class JavaEntity
    {
        public AccessModifier AccessModifier { get; }
        public string Name { get; }
        public bool IsFinal { get; }
        public bool IsStatic { get; }

        protected JavaEntity(AccessModifier accessModifier, string name, bool isFinal, bool isStatic)
        {
            AccessModifier = accessModifier;
            Name = name;
            IsFinal = isFinal;
            IsStatic = isStatic;
        }
    }
}
