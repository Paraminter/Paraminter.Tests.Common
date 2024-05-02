namespace Paraminter;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public static class CSharpCompilationFactory
{
    private static readonly CSharpCompilation EmptyCompilation = CreateEmptyCompilation();

    private static readonly CSharpParseOptions ParseOptions = new(languageVersion: LanguageVersion.CSharp11);
    private static readonly CSharpCompilationOptions CompilationOptions = new(OutputKind.DynamicallyLinkedLibrary);

    public static CSharpCompilation Create(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source, ParseOptions);

        return EmptyCompilation.AddSyntaxTrees(syntaxTree);
    }

    private static CSharpCompilation CreateEmptyCompilation()
    {
        var references = ListAssemblies()
            .Where(static (assembly) => assembly.IsDynamic is false)
            .Select(static (assembly) => MetadataReference.CreateFromFile(assembly.Location))
            .Cast<MetadataReference>();

        return CSharpCompilation.Create("FakeAssembly", references: references, options: CompilationOptions);
    }

    private static List<Assembly> ListAssemblies()
    {
        Queue<Assembly> unresolvedAssemblies = new();
        List<Assembly> resolvedAssemblies = [];

        HashSet<string> resolvedAssemblyNames = [];

        unresolvedAssemblies.Enqueue(Assembly.GetExecutingAssembly());

        while (unresolvedAssemblies.Count != 0)
        {
            var targetAssembly = unresolvedAssemblies.Dequeue();

            foreach (var assemblyName in targetAssembly.GetReferencedAssemblies().Where((assemblyName) => resolvedAssemblyNames.Contains(assemblyName.FullName) is false))
            {
                resolvedAssemblyNames.Add(assemblyName.FullName);

                Assembly assemblyReference;

                try
                {
                    assemblyReference = Assembly.Load(assemblyName);
                }
                catch (Exception e) when (e is FileLoadException or FileNotFoundException)
                {
                    continue;
                }

                unresolvedAssemblies.Enqueue(assemblyReference);
                resolvedAssemblies.Add(assemblyReference);
            }
        }

        resolvedAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());

        return resolvedAssemblies;
    }
}
