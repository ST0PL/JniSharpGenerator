using JniSharpGenerator.Parser;

namespace JniSharpGenerator
{
    /// <summary>
    /// Represents method data with a unique alias used for handling method overloads.
    /// </summary>
    /// <remarks>
    /// If no overload exists, Alias equals the method name. 
    /// If overloads are present, Alias is formatted with an index suffix (e.g., name0, name1,s name2...).
    /// </remarks>
    internal class MethodData(string alias, JavaMethod method)
    {
        public string Alias => alias;
        public JavaMethod Method => method;
    }
}
