using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions.Helpers
{
    internal static class SymbolExtensions
    {
        /// <summary>
        ///     Retrieves an attribute data based on its <paramref name="fullName"/>
        /// </summary>
        public static AttributeData GetAttribute(this ISymbol symbol, string fullName)
        {
            return symbol.GetAttributes(fullName).FirstOrDefault();
        }

        /// <summary>
        ///     Retrieves multiple attribute data based on its <paramref name="fullName"/>
        /// </summary>
        public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string fullName)
        {
            return symbol.GetAttributes().Where(a => a.AttributeClass?.ToString() == fullName);
        }

        /// <summary>
        ///     Retrieves an attribute constructor argument based on its <paramref name="index"/>
        /// </summary>
        public static T? GetConstructorArgument<T>(this AttributeData attributeData, int index)
        {
            return attributeData.ConstructorArguments.Length > index 
                && !attributeData.ConstructorArguments[index].IsNull ? (T)attributeData.ConstructorArguments[index].Value! : default;
        }

        /// <summary>
        ///     Retrieves an attribute constructor array argument based on its <paramref name="index"/>
        /// </summary>
        public static IEnumerable<T>? GetArrayConstructorArgument<T>(this AttributeData attributeData, int index)
        {
            if (attributeData.ConstructorArguments.Length <= index) { return null; }

            TypedConstant typedConstant = attributeData.ConstructorArguments[index];
            return typedConstant.Kind == TypedConstantKind.Array ? typedConstant.Values.Select(x => (T)x.Value!) : null;
        }

        /// <summary>
        ///     Retrieves an attribute named argument based on its <paramref name="name"/>
        /// </summary>
        public static T? GetNamedArgument<T>(this AttributeData attributeData, string name)
        {
            TypedConstant typedConstant = attributeData.NamedArguments.FirstOrDefault(kp => kp.Key == name).Value;
            return !typedConstant.IsNull ? (T?)typedConstant.Value : default;
        }

        /// <summary>
        ///     Retrieves an attribute named array argument based on its <paramref name="name"/>
        /// </summary>
        public static IEnumerable<T>? GetArrayNamedArgument<T>(this AttributeData attributeData, string name)
        {
            TypedConstant typedConstant = attributeData.NamedArguments.FirstOrDefault(kp => kp.Key == name).Value;

            return typedConstant.Kind == TypedConstantKind.Array ? typedConstant.Values.Select(x => (T)x.Value!) : null;
        }

        /// <summary>
        ///     Retrieves the assembly name where <paramref name="typeSymbol"/> is located
        /// </summary>
        public static INamedTypeSymbol GetClosedTypeOrOpenGeneric(this INamedTypeSymbol symbol)
        {
            // If we are dealing with a generic type whose type arguments are the type parameters, just convert it to open generic
            if (symbol.IsGenericType && symbol.TypeArguments.SequenceEqual(symbol.TypeParameters, (symbol1, symbol2) => symbol1.ToString() == symbol2.ToString()))
            {
                return symbol.ConstructUnboundGenericType();
            }

            return symbol;
        }
    }
}
