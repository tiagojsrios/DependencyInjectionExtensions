using System.Linq;
using System.Text.RegularExpressions;

namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    ///     <see cref="string"/> extension methods
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        ///     Creates a safe identifier from <paramref name="value"/> that can be used, for example, on a method name
        /// </summary>
        public static string ToSafeIdentifier(this string value)
        {
            // Clean special chars
            value = Regex.Replace(value, "[^a-zA-Z0-9]+", " ", RegexOptions.Compiled).Trim();

            // Remove spaces
            value = value.Replace(" ", "");

            // Identifiers can't start with 0-9
            return char.IsDigit(value.FirstOrDefault()) ? $"_{value}" : value;
        }

        /// <summary>
        ///     Checks whether <paramref name="value"/> ends with <paramref name="suffix"/>. If not, the method will append it.
        /// </summary>
        public static string EnsureEndsWith(this string value, string suffix)
        {
            return value.EndsWith(suffix) ? value : value + suffix;
        }
    }
}
