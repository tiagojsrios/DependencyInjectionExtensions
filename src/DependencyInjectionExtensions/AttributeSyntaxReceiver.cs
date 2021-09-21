using DependencyInjectionExtensions.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionExtensions
{
    internal class AttributeSyntaxReceiver : ISyntaxReceiver
    {
        private readonly bool _partialOnly;
        private readonly string[] _attributeNames;

        public List<TypeDeclarationSyntax> Candidates { get; } = new List<TypeDeclarationSyntax>();

        public AttributeSyntaxReceiver(bool partialOnly, params string[] attributeNames)
        {
            _partialOnly = partialOnly;
            _attributeNames = attributeNames;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Type declarations only
            if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax) { return; }

            // Partial types only, for when we want extend the types
            if (_partialOnly && !typeDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword))) { return; }

            // Check if they have the attribute we are looking for
            var attributes = typeDeclarationSyntax.AttributeLists.SelectMany(attributeList => attributeList.Attributes);
            if (!_attributeNames.Intersect(attributes.Select(a => a.Name.ToString().Split('.').Last().EnsureEndsWith("Attribute"))).Any()) { return; }

            Candidates.Add(typeDeclarationSyntax);
        }
    }
}