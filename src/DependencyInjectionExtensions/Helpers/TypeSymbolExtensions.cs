using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions.Helpers
{
    /// <summary>
    ///     <see cref="ITypeSymbol"/> extension methods
    /// </summary>
    internal static class TypeSymbolExtensions
    {
        /// <summary>
        ///     Retrieves an attribute data based on its <paramref name="attributeName"/>
        /// </summary>
        public static AttributeData GetAttributeByName(this ITypeSymbol typeSymbol, string attributeName)
            => typeSymbol.GetAttributes().First(x => x.AttributeClass?.Name == attributeName);

        /// <summary>
        ///     Retrieves an attribute named argument based on its <paramref name="name"/>
        /// </summary>
        public static IEnumerable<T> GetArrayNamedArgument<T>(this AttributeData attributeData, string name)
        {
            TypedConstant typedConstant = attributeData.NamedArguments.FirstOrDefault(kp => kp.Key == name).Value;

            return typedConstant.IsNull
                ? Enumerable.Empty<T>()
                : typedConstant.Values.Select(x => (T)x.Value);
        }

        /// <summary>
        ///     Retrieves an attribute constructor argument based on its <paramref name="index"/>
        /// </summary>
        public static T GetConstructorArgument<T>(this AttributeData attributeData, int index = 0)
            => (T)attributeData.ConstructorArguments[index].Value;

        /// <summary>
        ///     Retrieves the assembly name where <paramref name="typeSymbol"/> is located
        /// </summary>
        public static string GetAssemblyName(this ITypeSymbol typeSymbol)
            => typeSymbol.ContainingAssembly.Name;
    }
}
