using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DRCDesigner.Business.Helpers
{
   public class Class1
    {
        public void a()
        {
            SyntaxFactory.CompilationUnit()
                .WithMembers(
                    SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                        SyntaxFactory.PropertyDeclaration(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.ByteKeyword)),
                                SyntaxFactory.Identifier("ByteMember"))
                            .WithAttributeLists(
                                SyntaxFactory.SingletonList<AttributeListSyntax>(
                                    SyntaxFactory.AttributeList(
                                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                            SyntaxFactory.Attribute(
                                                    SyntaxFactory.IdentifierName("ByteMember"))
                                                .WithArgumentList(
                                                    SyntaxFactory.AttributeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                                            SyntaxFactory.AttributeArgument(
                                                                    SyntaxFactory.LiteralExpression(
                                                                        SyntaxKind.NumericLiteralExpression,
                                                                        SyntaxFactory.Literal(5)))
                                                                .WithNameEquals(
                                                                    SyntaxFactory.NameEquals(
                                                                        SyntaxFactory
                                                                            .IdentifierName("DefaultValue"))))))))))
                            .WithAccessorList(
                                SyntaxFactory.AccessorList(
                                    SyntaxFactory.List<AccessorDeclarationSyntax>(
                                        new AccessorDeclarationSyntax[]
                                        {
                                            SyntaxFactory.AccessorDeclaration(
                                                    SyntaxKind.GetAccessorDeclaration)
                                                .WithSemicolonToken(
                                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                            SyntaxFactory.AccessorDeclaration(
                                                    SyntaxKind.SetAccessorDeclaration)
                                                .WithSemicolonToken(
                                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                        })))))
                .NormalizeWhitespace();
        }

        public T GetDefaultValue<T>( string defaultv)
        {
            return (T)Convert.ChangeType(defaultv, typeof(T));
        }

    }
}
