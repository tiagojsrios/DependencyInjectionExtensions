using DependencyInjectionExtensions.Helpers;
using DependencyInjectionExtensions.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

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
            }
        }
    }
}
