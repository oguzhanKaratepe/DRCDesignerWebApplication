using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DRCDesigner.Entities.Concrete;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DRCDesigner.Business.Helpers
{
   public class RoslynDocumentCodeGenerator
    {


        public NamespaceDeclarationSyntax generateNamespaceDeclaration(string nameSpace)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Error with your namespace string")).NormalizeWhitespace();
            if (nameSpace != null)
            {
                @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(nameSpace)).NormalizeWhitespace();
            }

            return @namespace;
        }

        public InterfaceDeclarationSyntax generateDocumentInterface(string className,string comment)
        {
            className = camelCaseDocumentName(className);

            return SyntaxFactory.InterfaceDeclaration("I" + className)
                .WithAttributeLists(
                    SyntaxFactory.SingletonList<AttributeListSyntax>(
                        SyntaxFactory.AttributeList(
                            SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                SyntaxFactory.Attribute(
                                        SyntaxFactory.IdentifierName("Document"))
                                    .WithArgumentList(
                                        SyntaxFactory.AttributeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                                SyntaxFactory.AttributeArgument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal(className))))))))

                .WithOpenBracketToken(
                    SyntaxFactory.Token(generateSummaryComment(comment), SyntaxKind.OpenBracketToken,
                        SyntaxFactory.TriviaList()))))


                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    ))
                .WithBaseList(
                    SyntaxFactory.BaseList(
                        SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.IdentifierName("IDocument")))))
                .NormalizeWhitespace();



        }
        public InterfaceDeclarationSyntax generateShadowDocumentInterface(string className, string sourceDocumentName,string comment)
        {
            return SyntaxFactory.InterfaceDeclaration("I" + className)
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(generateSummaryComment(comment), SyntaxKind.PublicKeyword, SyntaxFactory.TriviaList())))
                .WithBaseList(
                    SyntaxFactory.BaseList(
                        SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.IdentifierName("I" + sourceDocumentName)))))
                .NormalizeWhitespace();



        }

        //interface naming for complex, detailelement and dynamic field
        public InterfaceDeclarationSyntax generateDetailComplexDynamicDocumentInterface(string className, Field field)
        {

            string superInterface = "";
            switch (field.Type)
            {
                case FieldType.ComplexTypeElement:
                    superInterface = "IComplexTypeElement";
                    break;
                case FieldType.DetailElement:
                    superInterface = "IDocumentDetailElement";
                    break;
                case FieldType.DynamicField:
                    superInterface = "IDocumentDynamicFieldElement";
                    break;
            }
            return SyntaxFactory.InterfaceDeclaration(className)
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(generateSummaryComment(field.Description), SyntaxKind.PublicKeyword, SyntaxFactory.TriviaList())))
                .WithBaseList(
                    SyntaxFactory.BaseList(
                        SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.IdentifierName(superInterface)))))
                .NormalizeWhitespace();



        }
        public EnumDeclarationSyntax generateEnumDeclaration(Field enumField)
        {


            var enumDeclaretion =
                SyntaxFactory.EnumDeclaration(enumField.ItemName)
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(
                                generateSummaryComment(enumField.Description),
                                SyntaxKind.PublicKeyword,
                                SyntaxFactory.TriviaList())));

            return enumDeclaretion;


        }

        public PropertyDeclarationSyntax generateDocumentPropertiesDeclarationWithAttributes(Field field)
        {
            //if nullable I need to add ? sign after property type
            if (field.Nullable)
            {
                var attributelist = NormalPropertyAtributes(field);
                
                var fieldbase = SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.NullableType(
                            SyntaxFactory.IdentifierName(getNormalFieldType(field))),
                        SyntaxFactory.Identifier(field.AttributeName))

                    .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                        attributelist
                    ))
                    
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
                                })))
                    .NormalizeWhitespace();

                return fieldbase;
            }
            else
            {
                var fieldbase = SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.NullableType(
                            SyntaxFactory.IdentifierName(getNormalFieldType(field))),
                        SyntaxFactory.Identifier(field.AttributeName))

                    .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                        NormalPropertyAtributes(field)
                    ))

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
                                })))
                    .NormalizeWhitespace();
             
                return fieldbase;
            }
           
        }
        public PropertyDeclarationSyntax generateDocumentDexmoPropertiesDeclarationWithAttributes(Field field)
        {
            var propertyName = cleanDetailElementName(field.AttributeName); // Lines[] will convert lines

            if (field.Nullable)
            {
               var  fieldbase = SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.NullableType(
                            SyntaxFactory.IdentifierName(GetComplexFieldType(field))),
                        SyntaxFactory.Identifier(propertyName))


                    .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                            complexPropertyAtributes(field)
                        )
                    )
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
                                })))
                    .NormalizeWhitespace();

               return fieldbase;
            }
            else
            {
              var  fieldbase = SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.IdentifierName(GetComplexFieldType(field)),
                        SyntaxFactory.Identifier(propertyName))

                    .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                        complexPropertyAtributes(field)
                    ))

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
                                })))
                    .NormalizeWhitespace();

              return fieldbase;
            }

        }

        public PropertyDeclarationSyntax generateDocumentRelationPropertyDeclarationWithAttributes(Field field)
        {
            var mainProperty = SyntaxFactory.PropertyDeclaration(
                               SyntaxFactory.PredefinedType(
                                   SyntaxFactory.Token(
                                       SyntaxFactory.TriviaList(),
                                       SyntaxKind.StringKeyword,
                                       SyntaxFactory.TriviaList(
                                           SyntaxFactory.Space))),
                               SyntaxFactory.Identifier(
                                   SyntaxFactory.TriviaList(),
                                   //key point
                                   field.AttributeName + "Id",
                                   SyntaxFactory.TriviaList(
                                       SyntaxFactory.Space)))

                           .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                               NormalPropertyAtributes(field)
                           ))

                           .WithAccessorList(
                               SyntaxFactory.AccessorList(
                                       SyntaxFactory.List<AccessorDeclarationSyntax>(
                                           new AccessorDeclarationSyntax[]
                                           {
                                                    SyntaxFactory.AccessorDeclaration(
                                                            SyntaxKind.GetAccessorDeclaration)
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(
                                                                SyntaxFactory.TriviaList(),
                                                                SyntaxKind.SemicolonToken,
                                                                SyntaxFactory.TriviaList(
                                                                    SyntaxFactory.Space))),
                                                    SyntaxFactory.AccessorDeclaration(
                                                            SyntaxKind.SetAccessorDeclaration)
                                                        .WithSemicolonToken(
                                                            SyntaxFactory.Token(
                                                                SyntaxFactory.TriviaList(),
                                                                SyntaxKind.SemicolonToken,
                                                                SyntaxFactory.TriviaList(
                                                                    SyntaxFactory.Space)))
                                           }))
                                   .WithOpenBraceToken(
                                       SyntaxFactory.Token(
                                           SyntaxFactory.TriviaList(),
                                           SyntaxKind.OpenBraceToken,
                                           SyntaxFactory.TriviaList(
                                               SyntaxFactory.Space)))
                                   .WithCloseBraceToken(
                                       SyntaxFactory.Token(
                                           SyntaxFactory.TriviaList(),
                                           SyntaxKind.CloseBraceToken,
                                           SyntaxFactory.TriviaList(
                                               SyntaxFactory.LineFeed))));

            return mainProperty;
        }

        public EnumMemberDeclarationSyntax generateEnumProperty(String enumMember,int value,int totalMemberCount)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            var member = SyntaxFactory.EnumMemberDeclaration(
                    SyntaxFactory.Identifier(textInfo.ToTitleCase(enumMember)))
                .WithEqualsValue(
                    SyntaxFactory.EqualsValueClause(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(value))));

            member.AddModifiers(SyntaxFactory.Token(SyntaxKind.CommaToken));


            if (totalMemberCount == value)
            {

                member.ReplaceToken(SyntaxFactory.Token(SyntaxKind.CommaToken), SyntaxFactory.Token(SyntaxKind.NullKeyword));

            }


            return member;
        }
        public PropertyDeclarationSyntax generateDocumentRelationForeignKeyPropertyDeclaration(Field field, string relatedDocumentName)
        {
            var relationKey = SyntaxFactory.PropertyDeclaration(
                                              SyntaxFactory.IdentifierName(
                                                  SyntaxFactory.Identifier(
                                                      SyntaxFactory.TriviaList(
                                                          SyntaxFactory.LineFeed),
                                                      //key point
                                                      "I" + relatedDocumentName,
                                                      SyntaxFactory.TriviaList(
                                                          SyntaxFactory.Space))),
                                              SyntaxFactory.Identifier(
                                                  SyntaxFactory.TriviaList(),
                                                  //key point 
                                                  field.AttributeName,
                                                  SyntaxFactory.TriviaList(
                                                      SyntaxFactory.Space)))

                                          .WithAccessorList(
                                              SyntaxFactory.AccessorList(
                                                      SyntaxFactory.SingletonList<AccessorDeclarationSyntax>(
                                                          SyntaxFactory.AccessorDeclaration(
                                                                  SyntaxKind.GetAccessorDeclaration)
                                                              .WithSemicolonToken(
                                                                  SyntaxFactory.Token(
                                                                      SyntaxFactory.TriviaList(),
                                                                      SyntaxKind.SemicolonToken,
                                                                      SyntaxFactory.TriviaList(
                                                                          SyntaxFactory.Space)))))
                                                  .WithOpenBraceToken(
                                                      SyntaxFactory.Token(
                                                          SyntaxFactory.TriviaList(),
                                                          SyntaxKind.OpenBraceToken,
                                                          SyntaxFactory.TriviaList(
                                                              SyntaxFactory.Space)))
                                                  .WithCloseBraceToken(
                                                      SyntaxFactory.Token(
                                                          SyntaxFactory.TriviaList(),
                                                          SyntaxKind.CloseBraceToken,
                                                          SyntaxFactory.TriviaList(
                                                              SyntaxFactory.LineFeed))));

            return relationKey;
        }

        public SyntaxTriviaList generateSummaryComment(string comment)
        {
            if (comment == null)
            {
                comment = "ERROR, your comment string is null";
            }
            return
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
                                            SyntaxFactory.DocumentationCommentExterior("///")),
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
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        "\n",
                                                        "\n",
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextLiteral(
                                                        SyntaxFactory.TriviaList(
                                                            SyntaxFactory.DocumentationCommentExterior("    ///")),
                                                        comment,
                                                        comment,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        "\n",
                                                        "\n",
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextLiteral(
                                                        SyntaxFactory.TriviaList(
                                                            SyntaxFactory.DocumentationCommentExterior("   ///")),
                                                        " ",
                                                        " ",
                                                        SyntaxFactory.TriviaList())
                                                }))))
                            .WithStartTag(
                                SyntaxFactory.XmlElementStartTag(
                                    SyntaxFactory.XmlName(
                                        SyntaxFactory.Identifier("summary"))))
                            .WithEndTag(
                                SyntaxFactory.XmlElementEndTag(
                                    SyntaxFactory.XmlName(
                                        SyntaxFactory.Identifier("summary")))),
                        SyntaxFactory.XmlText()
                            .WithTextTokens(
                                SyntaxFactory.TokenList(
                                    SyntaxFactory.XmlTextNewLine(
                                        SyntaxFactory.TriviaList(),
                                        "\n",
                                        "\n",
                                        SyntaxFactory.TriviaList())))
                            }))));
        }
        private string cleanDetailElementName(string attributeName)
        {
            string[] words = attributeName.Split("[");

            return words[0].ToLower().Trim();
        }

        
        
            private string getNormalFieldType(Field field)
        {
            switch (field.Type)
            {
                case FieldType.Bool:
                    return "bool";

                case FieldType.String:
                    return "string";

                case FieldType.Byte:
                    return "byte";

                case FieldType.DateOnly:
                    return "DateTime";

                case FieldType.DateTime:
                    return "DateTimeOffset";

                case FieldType.Time:
                    return "TimeSpan";

                case FieldType.Decimal:
                    return "decimal";

                case FieldType.Double:
                    return "double";

                case FieldType.Integer:
                    return "int";

                case FieldType.Long:
                    return "long";

                default:
                    return field.Type.ToString();

            }
        }
        private AttributeListSyntax[] NormalPropertyAtributes(Field field)
        {
            //25 above my upper bound for attribute limit
            var a = new AttributeListSyntax[25];
            int point = 0;

            if (field.Required)
            {
                a = fieldValueWithoutInstance(a, point, "Required");
                point++;
            }

            if (field.CreditCard)
            {
                a = fieldValueWithoutInstance(a, point, "CreditCard");
                point++;
            }

            if (field.Unique)
            {
                a = fieldValueWithoutInstance(a, point, "Unique");
                point++;
            }
            if (field.MaxLength != null)
            {
                a[point] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("MaxLength"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal((int)field.MaxLength))))))));
                point++;
            }
            if (field.MinLength != null)
            {
                a[point] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("MinLength"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal((int)field.MinLength))))))));
                point++;
            }
            if (field.MaxValue != null)
            {
                a[point] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("MaxValue"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal((double)field.MaxValue))))))));
                point++;
            }
            if (field.MinValue != null)
            {
                a[point] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("MinValue"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal((double)field.MinValue))))))));
                point++;
            }
            if (field.RegularExpression != null)
            {
                a[point] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("RegularExpression"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(field.RegularExpression))))))));
                    

                point++;
            }
            if (field.Type == FieldType.Integer)
            {
                a = fieldValueWithoutInstance(a, point, "IntegerMember", field);
                point++;
            }
            if (field.Type == FieldType.String)
            {
                a = fieldValueWithoutInstance(a, point, "StringMember", field);
                point++;
            }
            if (field.Type == FieldType.Long)
            {
                a = fieldValueWithoutInstance(a, point, "LongMember", field);
                point++;
            }
            if (field.Type == FieldType.Double)
            {
                a = fieldValueWithoutInstance(a, point, "DoubleMember", field);
                point++;
            }
            if (field.Type == FieldType.Bool)
            {
                a = fieldValueWithoutInstance(a, point, "BooleanMember", field);
                point++;
            }
            if (field.Type == FieldType.Byte)
            {
                a = fieldValueWithoutInstance(a, point, "ByteMember", field);
                point++;
            }
            if (field.Type == FieldType.Decimal)
            {
                a = fieldValueWithoutInstance(a, point, "DecimalMember", field);
                point++;
            }
            if (field.Type == FieldType.DateOnly)
            {
                a = fieldValueWithoutInstance(a, point, "DateOnlyMember", field);
                point++;
            }
            if (field.Type == FieldType.DateTime)
            {
                a = fieldValueWithoutInstance(a, point, "DateTimeMember", field);
                point++;
            }
            if (field.Type == FieldType.Time)
            {
                a = fieldValueWithoutInstance(a, point, "TimeMember", field);
                point++;
            }
            if (field.Type == FieldType.RelationElement)
            {
                a = fieldValueWithoutInstance(a, point, "RelationElement", field);
                point++;
            }

            var b = new AttributeListSyntax[point];

        
            for (int i = 0; i < point; i++)
            {
                b[i] = a[i];
              
            }

            if (b.Length > 0)
            {
                b[0] = b[0].WithOpenBracketToken(SyntaxFactory.Token(generateSummaryComment(field.Description), SyntaxKind.OpenBracketToken,
                    SyntaxFactory.TriviaList()));
            }
           

            return b;
        }

        private AttributeListSyntax[] fieldValueWithoutInstance(AttributeListSyntax[] a, int index, string atributeName, Field field)
        {
            if (field.DefaultValue != null)
            {
                a[index] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName(atributeName))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(field.DefaultValue)))
                                            .WithNameEquals(
                                                SyntaxFactory.NameEquals(
                                                    SyntaxFactory.IdentifierName("DefaultValue"))))))));
            }
            else if (field.Type == FieldType.RelationElement)
            {
                a[index] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName(atributeName))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.IdentifierName("nameof"))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName(
                                                                    field.AttributeName + "Id")))))))))));
            }
            else
            {
                a[index] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName(atributeName))));

            }


            return a;
        }
        private AttributeListSyntax[] fieldValueWithoutInstance(AttributeListSyntax[] a, int index, string attributeName)
        {

            a[index] = SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                    SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName(attributeName))));



            return a;
        }
        private string GetComplexFieldType(Field field)
        {
            if (field.Type == FieldType.Enum)
            {
                return camelCaseDocumentName(field.ItemName);
            }
            else if (field.Type == FieldType.DetailElement)
            {


                try
                {
                    string outputToReturn = "IDocumentDetailElementCollection<" + field.ItemName + ">";
                    return outputToReturn;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }


            }
            else if (field.Type == FieldType.ComplexTypeElement || field.Type == FieldType.Enum)
            {
                return field.ItemName;
            }
            else if (field.Type == FieldType.DynamicField)
            {
                try
                {
                    string outputToReturn = "IDocumentDetailElementCollection<" + field.ItemName + ">";
                    return outputToReturn;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }
            }
            else
            {
                return "SomeThingWrong";
            }

            return "SomeThingWrong";
        }

        private AttributeListSyntax[] complexPropertyAtributes(Field field)
        {
            //25 above my upper bound for attribute limit
            var a = new AttributeListSyntax[10];
            int point = 0;

            if (field.Required)
            {
                a = fieldValueWithoutInstance(a, point, "Required");
                point++;
            }

            if (field.Type == FieldType.Enum)
            {
                a = fieldValueWithoutInstance(a, point, "EnumMember");
                point++;
            }

            if (field.Type == FieldType.ComplexTypeElement)
            {
                a = fieldValueWithoutInstance(a, point, "ComplexTypeMember");
                point++;
            }

            if (field.Type == FieldType.DetailElement || field.Type == FieldType.DynamicField)
            {
                a = fieldValueWithoutInstance(a, point, "DocumentDetailMember");
                point++;
            }


            var b = new AttributeListSyntax[point];

            for (int i = 0; i < point; i++)
            {
                b[i] = a[i];

            }

            b[0] = b[0].WithOpenBracketToken(SyntaxFactory.Token(generateSummaryComment(field.Description), SyntaxKind.OpenBracketToken,
                SyntaxFactory.TriviaList()));
            return b;
        }

        private string camelCaseDocumentName(string className)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            className = className.ToLower();
            className = myTI.ToTitleCase(className);
            className = className.Replace(" ", "").Trim();

            return className;
        }

    }
}
