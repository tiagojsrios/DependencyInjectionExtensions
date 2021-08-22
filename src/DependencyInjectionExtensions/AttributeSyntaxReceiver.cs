using DependencyInjectionExtensions.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions
{
    /// <inheritdoc cref="ISyntaxReceiver"/>
    internal class AttributeSyntaxReceiver : ISyntaxReceiver
    {
        private readonly string[] _attributeNames;

        public List<TypeDeclarationSyntax> Candidates { get; } = new();
        
        /// <summary>
        ///     <see cref="AttributeSyntaxReceiver"/> ctor
        /// </summary>
        /// <param name="attributeNames"></param>
        public AttributeSyntaxReceiver(params string[] attributeNames)
        {
            _attributeNames = attributeNames;
        }

        /// <inheritdoc cref="ISyntaxReceiver.OnVisitSyntaxNode"/>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Type declarations only
            if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax) { return; }

            // Check if they have the attribute we are looking for
            IEnumerable<AttributeSyntax> attributes = typeDeclarationSyntax
                .AttributeLists
                .SelectMany(attributeList => attributeList.Attributes);
            
            if (!_attributeNames.Intersect(attributes.Select(a => a.Name.ToString().Split('.').Last().EnsureEndsWith("Attribute"))).Any()) { return; }

            Candidates.Add(typeDeclarationSyntax);
        }
    }
}
