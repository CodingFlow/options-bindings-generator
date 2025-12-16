using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodingFlow.OptionsBindingsGenerator
{
    [Generator]
    public class Main : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var types = context.SyntaxProvider.ForAttributeWithMetadataName(
                $"CodingFlow.OptionsBindingsGenerator.{nameof(OptionsBindingsAttribute)}",
                predicate: IsSyntaxTargetForGeneration,
                transform: GetSemanticTargetForGeneration
            );

            var allTypes = types.Collect();

            context.RegisterSourceOutput(allTypes, Execute);
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode, CancellationToken token)
        {
            return syntaxNode is RecordDeclarationSyntax || syntaxNode is ClassDeclarationSyntax;
        }

        private static INamedTypeSymbol GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
        {
            return context.TargetSymbol as INamedTypeSymbol;
        }

        private static void Execute(SourceProductionContext context, ImmutableArray<INamedTypeSymbol> typeSymbols)
        {
            if (typeSymbols.Any())
            {
                var (source, className) = OutputGenerator.GenerateOutput(typeSymbols);

                context.AddSource($"{className}.generated.cs", SourceText.From(source, Encoding.UTF8, SourceHashAlgorithm.Sha256));
            }
        }
    }
}