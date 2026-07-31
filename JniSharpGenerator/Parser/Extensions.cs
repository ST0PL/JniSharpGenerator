using System.Text.RegularExpressions;

namespace JniSharpGenerator.Parser
{
    internal static class Extensions
    {
        extension(string value)
        {
            public T ToEnumOrDefault<T>(bool ignoreCase = false) where T : struct, Enum
                => Enum.TryParse(value, ignoreCase, out T result) ? result : default;
            public bool IsMatch(string pattern, RegexOptions options = default)
                => Regex.IsMatch(value, pattern, options);
        }
    }
}
