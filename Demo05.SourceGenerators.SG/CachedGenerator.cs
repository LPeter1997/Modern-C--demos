using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Demo05.SourceGenerators.SG
{
    [Generator]
    public class CachedGenerator : ISourceGenerator
    {
        // Collects syntax nodes for us, a "rough" sift
        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<MethodDeclarationSyntax> Syntaxes { get; set; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is MethodDeclarationSyntax method && method.AttributeLists.Count > 0)
                {
                    Syntaxes.Add(method);
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

            var compilation = context.Compilation;
            // We load the attribute to check symbolic equality
            var cachedAttribute = compilation.GetTypeByMetadataName("Demo05.SourceGenerators.CachedAttribute");

            // Only keep methods that has this attribute
            var methodsToCache = new List<IMethodSymbol>();
            foreach (var methodSyntax in receiver.Syntaxes)
            {
                var model = compilation.GetSemanticModel(methodSyntax.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(methodSyntax) as IMethodSymbol;
                var appliedAttributeTypes = symbol.GetAttributes().Select(attr => attr.AttributeClass);

                if (!appliedAttributeTypes.Any(attr => SymbolEqualityComparer.Default.Equals(attr, cachedAttribute)))
                {
                    // No [Cached]
                    continue;
                }

                // It is [Cached]
                methodsToCache.Add(symbol);
            }

            // Now we generate implementations
            foreach (var methodSymbol in methodsToCache)
            {
                // We get the symbol that the method lies in
                var containingClass = methodSymbol.ContainingType;

                // A little helper to inject static, if the method is
                var staticAccess = methodSymbol.IsStatic ? "static" : string.Empty;

                // Key type is the tuple of argument types
                var keyType = string.Join(", ", methodSymbol.Parameters.Select(symbol => symbol.Type.ToDisplayString()));
                if (methodSymbol.Parameters.Length > 1) keyType = $"({keyType})";

                // Value type os the return type
                var valueType = methodSymbol.ReturnType.ToDisplayString();

                // Accessibility of the generated method will be the same
                var accessibility = methodSymbol.DeclaredAccessibility.ToString().ToLower();

                // Parameter list
                var parameterList = string.Join(", ", methodSymbol.Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}"));

                // The key list from the passed in parameters
                // We use these as keys in the dictionary
                var keyList = string.Join(", ", methodSymbol.Parameters.Select(p => p.Name));
                if (methodSymbol.Parameters.Length > 1) keyList = $"({keyList})";

                // The arguments to pass in for the method invocation
                var argumentList = string.Join(", ", methodSymbol.Parameters.Select(p => p.Name));

                // We generate the implementation, assuming the container is partial
                // We generate a friendly name for each implementation source-file
                var fileName = $"{containingClass.Name}.{methodSymbol.Name}.Generated.cs";
                context.AddSource(
                    fileName,
                    @$"
namespace {containingClass.ContainingNamespace.ToDisplayString()}
{{
    partial class {containingClass.Name}
    {{
        private {staticAccess} System.Collections.Generic.Dictionary<{keyType}, {valueType}> {methodSymbol.Name}_cachedValues = new();

        {accessibility} {staticAccess} {valueType} Cached{methodSymbol.Name}({parameterList})
        {{
            if (!{methodSymbol.Name}_cachedValues.TryGetValue({keyList}, out var result))
            {{
                result = {methodSymbol.Name}({argumentList});
                {methodSymbol.Name}_cachedValues.Add({keyList}, result);
            }}
            return result;
        }}
    }}
}}
");
            }
        }
    }
}
