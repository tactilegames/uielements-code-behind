using System;
using System.Linq;

namespace TactileModules.UIElementsCodeBehind
{
    internal static class StringExtensions
    {
        public static string ToPascalCase(this string value)
        {
            var parts = value.Split(new []{'-'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Select(x => $"{x.Substring(0, 1).ToUpper()}{x.Substring(1).ToLower()}");
            
            return string.Join(string.Empty, parts);
        }
    }
}