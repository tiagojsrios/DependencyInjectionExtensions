using DependencyInjectionExtensions.Helpers;
using DependencyInjectionExtensions.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionExtensions.Generators
{
    [Generator]
    public class OptionsGenerator : ISourceGenerator
    {
        public const string OptionsAttribute = nameof(OptionsAttribute);

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(ctx => ctx.AddSource($"{OptionsAttribute}.generated.cs", EmbeddedResourceHelper.GetEmbeddedResource($"{OptionsAttribute}.cs")));
            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver(false, OptionsAttribute));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not AttributeSyntaxReceiver syntaxReceiver) { return; }

            List<OptionsRegistrations> options = new();

            foreach (var candidateTypeNode in syntaxReceiver.Candidates)
            {
                SemanticModel model = context.Compilation.GetSemanticModel(candidateTypeNode.SyntaxTree);
                INamedTypeSymbol type = ModelExtensions.GetDeclaredSymbol(model, candidateTypeNode) as INamedTypeSymbol;

                if (type == null) { continue; }

                AttributeData attribute = type.GetAttribute($"DependencyInjectionExtensions.Attributes.{OptionsAttribute}");

                string configurationSectionName = attribute.GetNamedArgument<string>("ConfigurationSectionName");
                bool validateDataAnnotations = attribute.GetNamedArgument<bool>("ValidateDataAnnotations");

                options.Add(new OptionsRegistrations(type.ToDisplayString(), configurationSectionName ?? type.Name, validateDataAnnotations));
            }

            context.AddSource("ServiceCollectionExtensions.generated.cs", GenerateOptionsServiceCollection(context.Compilation.AssemblyName, options));
        }

        private static SourceText GenerateOptionsServiceCollection(string @namespace, IEnumerable<OptionsRegistrations> options)
        {
            return SyntaxFactory
                .ParseCompilationUnit(OptionsRegistrations.RenderClass(@namespace, options))
                .NormalizeWhitespace()
                .GetText(Encoding.UTF8);
        }
    }
}
