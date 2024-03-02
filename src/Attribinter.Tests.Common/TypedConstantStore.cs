namespace Attribinter;

using Microsoft.CodeAnalysis;

using System.Threading.Tasks;

public static class TypedConstantStore
{
    private static int Current;

    public static async Task<TypedConstant> GetNext()
    {
        var source = $$"""
            public class CustomAttribute : Attribute
            {
                public CustomAttribute(int value) { }
            }

            [CustomAttribute({{Current}})]
            public class Foo { }
            """;

        Current += 1;

        var (_, attributeData, _) = await CSharpCompilationStore.GetComponents(source, "Foo");

        return attributeData.ConstructorArguments[0];
    }
}
