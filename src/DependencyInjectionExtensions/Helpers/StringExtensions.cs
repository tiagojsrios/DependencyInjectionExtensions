namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, string suffix)
        {
            return str.EndsWith(suffix) ? str : str + suffix;
        }

        public static string ConditionalReplace(this string str, string oldValue, string newValue, bool condition)
        {
            return condition ? str.Replace(oldValue, newValue) : str;
        }
    }
}
