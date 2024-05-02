namespace Paraminter;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;
using System.Collections.Immutable;

public sealed class CustomAttributeData : AttributeData
{
    new public INamedTypeSymbol? AttributeClass { get; init; }
    new public IMethodSymbol? AttributeConstructor { get; init; }

    new public ImmutableArray<TypedConstant> ConstructorArguments { get; init; }
    new public ImmutableArray<KeyValuePair<string, TypedConstant>> NamedArguments { get; init; }

    new public SyntaxReference? ApplicationSyntaxReference { get; init; }

    protected override INamedTypeSymbol? CommonAttributeClass => AttributeClass;
    protected override IMethodSymbol? CommonAttributeConstructor => AttributeConstructor;

    protected override ImmutableArray<TypedConstant> CommonConstructorArguments => ConstructorArguments;
    protected override ImmutableArray<KeyValuePair<string, TypedConstant>> CommonNamedArguments => NamedArguments;

    protected override SyntaxReference? CommonApplicationSyntaxReference => ApplicationSyntaxReference;
}
