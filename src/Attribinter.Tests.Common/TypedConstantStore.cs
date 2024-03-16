namespace Attribinter;

using Microsoft.CodeAnalysis;

public static class TypedConstantStore
{
    private static int Current;

    public static TypedConstant GetNext()
    {
        var source = $$"""
            public class CustomAttribute : System.Attribute
            {
                public CustomAttribute(int value) { }
            }

            [CustomAttribute({{Current}})]
            public class Foo { }
            """;

        Current += 1;

        var compilation = CSharpCompilationFactory.Create(source);

        return compilation.GetTypeByMetadataName("Foo")!.GetAttributes()[0].ConstructorArguments[0];
    }
}
