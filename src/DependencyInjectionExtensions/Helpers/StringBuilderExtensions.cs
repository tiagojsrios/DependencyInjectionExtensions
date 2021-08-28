using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    ///     <see cref="StringBuilder"/> extension methods
    /// </summary>
    internal static class StringBuilderExtensions
    {
        /// <summary>
        ///     Method that receives a collection of <see cref="string"/> that should be appended to <paramref name="builder"/>
        /// </summary>
        public static StringBuilder AppendMultiple(this StringBuilder builder, IEnumerable<string> linesToAppend)
        {
            linesToAppend.ToList().ForEach(str => builder.Append(str));
            return builder;
        }
    }
}
