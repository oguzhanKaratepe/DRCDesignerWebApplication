using System;
using System.Collections.Generic;
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
                                SyntaxFactory.IdentifierName("Icomplex"),
                                SyntaxFactory.Identifier("AccountId"))
                            .WithAttributeLists(
                                SyntaxFactory.SingletonList<AttributeListSyntax>(
                                    SyntaxFactory.AttributeList(
                                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                                SyntaxFactory.Attribute(
                                                    SyntaxFactory.IdentifierName("Required"))))
                                        .WithOpenBracketToken(
                                            SyntaxFactory.Token(
                                                SyntaxFactory.TriviaList(
                                                    SyntaxFactory.Trivia(
                                                        SyntaxFactory.DocumentationCommentTrivia(
                                                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                                                            SyntaxFactory.List<XmlNodeSyntax>(
                                                                new XmlNodeSyntax[]
                                                                {
                                                                    SyntaxFactory.XmlText()
                                                                        .WithTextTokens(
                                                                            SyntaxFactory.TokenList(
                                                                                SyntaxFactory.XmlTextLiteral(
                                                                                    SyntaxFactory.TriviaList(
                                                                                        SyntaxFactory
                                                                                            .DocumentationCommentExterior(
                                                                                                "///")),
                                                                                    " ",
                                                                                    " ",
                                                                                    SyntaxFactory.TriviaList()))),
                                                                    SyntaxFactory.XmlExampleElement(
                                                                            SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                                                                SyntaxFactory.XmlText()
                                                                                    .WithTextTokens(
                                                                                        SyntaxFactory.TokenList(
                                                                                            new[]
                                                                                            {
                                                                                                SyntaxFactory
                                                                                                    .XmlTextNewLine(
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList(),
                                                                                                        "\n",
                                                                                                        "\n",
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList()),
                                                                                                SyntaxFactory
                                                                                                    .XmlTextLiteral(
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList(
                                                                                                                SyntaxFactory
                                                                                                                    .DocumentationCommentExterior(
                                                                                                                        "        ///")),
                                                                                                        " It specifies account Identity of sales invoice",
                                                                                                        " It specifies account Identity of sales invoice",
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList()),
                                                                                                SyntaxFactory
                                                                                                    .XmlTextNewLine(
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList(),
                                                                                                        "\n",
                                                                                                        "\n",
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList()),
                                                                                                SyntaxFactory
                                                                                                    .XmlTextLiteral(
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList(
                                                                                                                SyntaxFactory
                                                                                                                    .DocumentationCommentExterior(
                                                                                                                        "        ///")),
                                                                                                        " ",
                                                                                                        " ",
                                                                                                        SyntaxFactory
                                                                                                            .TriviaList())
                                                                                            }))))
                                                                        .WithStartTag(
                                                                            SyntaxFactory.XmlElementStartTag(
                                                                                SyntaxFactory.XmlName(
                                                                                    SyntaxFactory
                                                                                        .Identifier("summary"))))
                                                                        .WithEndTag(
                                                                            SyntaxFactory.XmlElementEndTag(
                                                                                SyntaxFactory.XmlName(
                                                                                    SyntaxFactory
                                                                                        .Identifier("summary")))),
                                                                    SyntaxFactory.XmlText()
                                                                        .WithTextTokens(
                                                                            SyntaxFactory.TokenList(
                                                                                SyntaxFactory.XmlTextNewLine(
                                                                                    SyntaxFactory.TriviaList(),
                                                                                    "\n",
                                                                                    "\n",
                                                                                    SyntaxFactory.TriviaList())))
                                                                })))),
                                                SyntaxKind.OpenBracketToken,
                                                SyntaxFactory.TriviaList()))))
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


    }
}
