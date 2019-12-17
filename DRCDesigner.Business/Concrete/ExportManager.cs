using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace DRCDesigner.Business.Concrete
{
    public class ExportManager : IExportService
    {
        private IDrcUnitOfWork _drcUnitOfWork;
        private IConfiguration _configuration { get; }

        public ExportManager(IDrcUnitOfWork drcUnitOfWork, IConfiguration configuration)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _configuration = configuration;
        }

        public void generateSubdomainVersionDocuments(int subdomainVersionId)
        {
            var subdomainVersion = _drcUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(subdomainVersionId);
           
            foreach (var drcCard in subdomainVersion.DRCards)
            {
                string classString = CreateClass(drcCard);
                System.IO.File.WriteAllText(@"C:\Users\oguzhan.karatepe\Desktop\DocumentStore\" +"I"+camelCaseDocumentName(drcCard.DrcCardName) +".cs", classString);
            }
            
        }


        private string camelCaseDocumentName(string className)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            className = className.ToLower();
            className = myTI.ToTitleCase(className);
            className=className.Replace(" ", "").Trim();

            return className;
        }
       

        private InterfaceDeclarationSyntax DocumentNameWithAttribute(string className)
        {

         return   SyntaxFactory.InterfaceDeclaration("I" + className)
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
                                                        SyntaxFactory.Literal(className))))))))))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithBaseList(
                    SyntaxFactory.BaseList(
                        SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.IdentifierName("IDocument")))))
                .NormalizeWhitespace();



        }
        private InterfaceDeclarationSyntax ShadowDocumentNameWithAttribute(string className,string sourceDocumentName)
        {
            return SyntaxFactory.InterfaceDeclaration("I"+className)
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithBaseList(
                    SyntaxFactory.BaseList(
                        SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.IdentifierName("I"+sourceDocumentName)))))
                .NormalizeWhitespace();



        }

        private string extractDocumentName(Field field)
        {

            return null;
        }
        private IEnumerable<PropertyDeclarationSyntax> classPropertiesWithAllAttributes(int drcCardId)
        {
            List<PropertyDeclarationSyntax> properties=new List<PropertyDeclarationSyntax>();

            var documentFields = _drcUnitOfWork.FieldRepository.getDrcCardAllFields(drcCardId);

            foreach (var field in documentFields)
            {
            
                if (field.Type==FieldType.ComplexTypeElement|| field.Type == FieldType.DetailElement|| field.Type == FieldType.Enum)
                {
                    string atribute = complexAttributeName(field);
                    if ( atribute!= null)
                    {
                        var fieldbase = SyntaxFactory.PropertyDeclaration(
                                SyntaxFactory.IdentifierName(GetComplexFieldType(field)),
                                SyntaxFactory.Identifier(atribute))
                            .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                                propertyAtributes(field)
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
                        properties.Add(fieldbase);
                    }
                }
                else
                {
                    var attributeName = decideAttributeName(field);
                    var fieldbase = SyntaxFactory.PropertyDeclaration(
                            SyntaxFactory.IdentifierName(getFieldType(field)),
                            SyntaxFactory.Identifier(attributeName))
                        .WithAttributeLists(SyntaxFactory.List<AttributeListSyntax>(

                            propertyAtributes(field)
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
                    properties.Add(fieldbase);
                }
              

                
             
            }

            _drcUnitOfWork.Complete();
            return properties;
        }

        private string decideAttributeName(Field field)
        {
            
                return field.AttributeName;
                
             }

        private string complexAttributeName(Field field)
        {
            if (field.Type == FieldType.Enum)
            {
                return "EnumMember";
            }

            if (field.Type == FieldType.DetailElement)
            {
                string[] words = field.AttributeName.Split('[');

                    return words[0];
            }

            if (field.Type == FieldType.ComplexTypeElement)
            {
                string[] words = field.AttributeName.Split('<');

             
                    return words[0];
                

            }

            return null;
        }

        private AttributeListSyntax[] propertyAtributes(Field field)
        {
            //25 above my upper bound for attribute limit
            var a= new AttributeListSyntax[25];
            int point = 0;
            
            

            if (field.Required)
            {
                a=fieldValueWithoutInstance(a, point, "Required");
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
            if (field.MaxLength!=null)
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
            if (field.MinValue!= null)
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
            if (field.Type == FieldType.ComplexTypeElement)
            {
                a = fieldValueWithoutInstance(a, point, "ComplexTypeMember");
                point++;
            }
            if (field.Type == FieldType.DetailElement)
            {
                a = fieldValueWithoutInstance(a, point, "DocumentDetailMember");
                point++;
            }
            if (field.Type == FieldType.Integer)
            {
                a = fieldValueWithoutInstance(a, point,"IntegerMember",field);
                point++;
            }
            if (field.Type == FieldType.String)
            {
                a = fieldValueWithoutInstance(a, point, "StringMember", field);
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



            var b= new AttributeListSyntax[point];

            for (int i = 0; i <point; i++)
            {
                b[i] = a[i];
                
            }

            return b;

        }

        private AttributeListSyntax[] fieldValueWithoutInstance(AttributeListSyntax[] a,int index,string attributeName)
        {
           
            a[index]=SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                    SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName(attributeName))));


          
            return a;
        }
        private AttributeListSyntax[] fieldValueWithoutInstance(AttributeListSyntax[] a, int index, string atributeName,Field field)
        {
            if (field.DefaultValue!=null)
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
            else
            {
                a[index] = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName(atributeName))));
            }
            
           


            
            return a;
        }


        private string getFieldType(Field field)
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

        private string GetComplexFieldType(Field field)
        {
            if (field.Type == FieldType.Enum)
            {
                return camelCaseDocumentName(field.AttributeName);
            }
            else if(field.Type==FieldType.DetailElement)
            {
                string input = field.AttributeName;

                try
                {
                    string output = input.Split('[', ']')[1];
                    string outputToReturn = "IDocumentDetailElementCollection<" + output + "> ";
                    return outputToReturn;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    
                }
                
               
            }
            else if (field.Type == FieldType.ComplexTypeElement)
            {
                string input = field.AttributeName;

                try
                {
                    string output = input.Split('<', '>')[1];

                    return output;
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
        private string CreateClass(DrcCard document)
        {
            // Create a namespace: (namespace CodeGenerationSample)
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Dexmo.Middleware.WebGate.Test.DocumentStore")).NormalizeWhitespace();

            //// Add System using statement: (using System)
            //@namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            string className = camelCaseDocumentName(document.DrcCardName);

            InterfaceDeclarationSyntax classDeclaration;
            //  Create a class: (class Order)
            if (document.MainCardId == null)
            {
                classDeclaration = DocumentNameWithAttribute(className);
            }
            else
            {
                string shadowDocumentName = _drcUnitOfWork.DrcCardRepository.getDrcCardName((int)document.MainCardId);
                shadowDocumentName = camelCaseDocumentName(shadowDocumentName);
               classDeclaration = ShadowDocumentNameWithAttribute(className,shadowDocumentName);
            }

           
            var fields = classPropertiesWithAllAttributes(document.Id);


            foreach (var property in fields)
            {
                // Add the field, the property and method to the class.
                classDeclaration = classDeclaration.AddMembers(property);
            }

            var childs=findClasses(_drcUnitOfWork.FieldRepository.getDrcCardAllFields(document.Id));
            
            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classDeclaration);

            // Normalize and get code as string.
            var code = @namespace.NormalizeWhitespace().ToFullString();

            return code;
        }

        private List<InterfaceDeclarationSyntax> findClasses(IEnumerable<Field> fields)
        {
            List<InterfaceDeclarationSyntax> classes=new List<InterfaceDeclarationSyntax>();

            List<Field> DetailedComplexfields =new List<Field>();
            foreach (var field in fields)
            {
               
            }

            return null;
        }

        
        public string generateSubdomainVersionReportUrl(int subdomainId)
        {
            //example url
            //https://localhost:44347/DrcCards/presentation?subdomain=warehousemanagement&version=0.2.0

            string url = "Something went wrong";
            try
            {
                var webAddress = _configuration.GetSection("WebSettings").GetSection("DrcDesignerWebAddress").Value;

                var subdomainVersion = _drcUnitOfWork.SubdomainVersionRepository.GetById(subdomainId);
                string subdomainName = _drcUnitOfWork.SubdomainRepository.GetSubdomainName(subdomainVersion.SubdomainId);
                subdomainName=subdomainName.ToLower().Replace(" ", "");

                url = webAddress + "DrcCards/presentation?subdomain=" + subdomainName + "&version=" + subdomainVersion.VersionNumber;
            }
            catch (Exception e)
            {
                // ignored
            }


            return url;
        }
    }
}
