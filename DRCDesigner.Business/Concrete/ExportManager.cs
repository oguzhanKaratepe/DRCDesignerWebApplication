using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.Helpers;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using ICSharpCode.SharpZipLib.Zip;

namespace DRCDesigner.Business.Concrete
{
    public class ExportManager : IExportService
    {
        private IDrcUnitOfWork _drcUnitOfWork;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IHostingEnvironment _env { get; }

        private RoslynDocumentCodeGenerator _documentGenerator;
        public ExportManager(IDrcUnitOfWork drcUnitOfWork, IConfiguration configuration, IMapper mapper, IHostingEnvironment env)
        {
            _documentGenerator = new RoslynDocumentCodeGenerator();
            _drcUnitOfWork = drcUnitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _env = env;
        }

        public byte[] generateSubdomainVersionDocuments(int subdomainVersionId)
        {

            var subdomainVersion =
                _drcUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(subdomainVersionId);
            var subdomainNamespace =
                _drcUnitOfWork.SubdomainRepository.GetSubdomainNamespace(subdomainVersion.SubdomainId);



            string file = _env.WebRootPath + "\\temp.zip"; // in production, would probably want to use a GUID as the file name so that it is unique
            System.IO.FileStream fs = System.IO.File.Create(file);

            using (ZipOutputStream zip = new ZipOutputStream(fs))
            {

                byte[] data;
                ZipEntry entry;
                foreach (var drcCard in subdomainVersion.DRCards)
                {
                    string classString = GenerateClassString(drcCard, subdomainNamespace);
                    string filename = "I" + camelCaseDocumentName(drcCard.DrcCardName) + ".cs";
                    byte[] byteArray = Encoding.UTF8.GetBytes(classString);
                    data = byteArray.ToArray();

                    entry = new ZipEntry(filename);
                    entry.DateTime = System.DateTime.Now;
                    zip.PutNextEntry(entry);
                    zip.Write(data, 0, data.Length);
                }
                zip.Finish();
                zip.Close();

                fs.Dispose(); // must dispose of it
                fs = System.IO.File.OpenRead(file); // must re-open the zip file
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                System.IO.File.Delete(file);

                return data;

            }
        }



        private string GenerateClassString(DrcCard document, string subdomainNamespace)
        {
            //full document template with required libraries
            var fullDocument = _documentGenerator.newFullDocumentTemplate();

            // Create a namespace: (namespace CodeGeneration)
            var @namespace = _documentGenerator.generateNamespaceDeclaration(subdomainNamespace);


            //Document Fields
            var documentFields = _drcUnitOfWork.FieldRepository.getDrcCardAllFieldsWithoutTracking(document.Id).ToList();

            var mainDocument = generateMainDocumentCode(document, documentFields);


            var firstFields = cloneList(documentFields);
            var fieldlistlayer = cloneListByReshapingFieldAttribute(firstFields, 1);
            List<InterfaceDeclarationSyntax> firstLevelChildsOfMainDocument = generateChildInterfacesCodeOfMainDocument(fieldlistlayer);

            var secondFields = cloneList(documentFields);
            var fieldlistlayer2 = cloneListByReshapingFieldAttribute(secondFields, 2);
            List<InterfaceDeclarationSyntax> secondLevelChildsOfMainDocument = generateChildInterfacesCodeOfMainDocument(fieldlistlayer2);


            var thirdFields = cloneList(documentFields);
            var fieldlistlayer3 = cloneListByReshapingFieldAttribute(thirdFields, 3);
            List<InterfaceDeclarationSyntax> thirdLevelChildsOfMainDocument = generateChildInterfacesCodeOfMainDocument(fieldlistlayer3);


            var fourthFields = cloneList(documentFields);
            var fieldlistlayer4 = cloneListByReshapingFieldAttribute(fourthFields, 4);
            List<InterfaceDeclarationSyntax> fourthLevelChildsOfMainDocument = generateChildInterfacesCodeOfMainDocument(fieldlistlayer4);


            var fieldsForEnumGeneration = cloneList(documentFields);
            //I am planning to store enum first then I will compare that enums. Because same enum type could be use 
            List<EnumDeclarationSyntax> documentEnumsFromAllLayers = generateEnums(fieldsForEnumGeneration);




            // Add the classes to the namespace.
            @namespace = @namespace.AddMembers(mainDocument);

            foreach (var child in firstLevelChildsOfMainDocument)
            {
                @namespace = @namespace.AddMembers(child);
            }
            foreach (var child in secondLevelChildsOfMainDocument)
            {
                @namespace = @namespace.AddMembers(child);
            }
            foreach (var child in thirdLevelChildsOfMainDocument)
            {
                @namespace = @namespace.AddMembers(child);
            }
            foreach (var child in fourthLevelChildsOfMainDocument)
            {
                @namespace = @namespace.AddMembers(child);
            }
            foreach (var child in documentEnumsFromAllLayers)
            {
                @namespace = @namespace.AddMembers(child);
            }




            fullDocument = fullDocument.WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@namespace));
            return fullDocument.NormalizeWhitespace().ToFullString();
        }

        private InterfaceDeclarationSyntax generateMainDocumentCode(DrcCard document, IEnumerable<Field> drcCardFields)
        {

            InterfaceDeclarationSyntax classDeclaration;
            //  Create a class: (class Order)
            if (document.MainCardId == null)
            {
                classDeclaration = _documentGenerator.generateDocumentInterface(document.DrcCardName, document.Definition);
            }
            else
            {
                string shadowDocumentName = camelCaseDocumentName(document.DrcCardName);
                classDeclaration = _documentGenerator.generateShadowDocumentInterface(document.DrcCardName, shadowDocumentName, document.Definition);
            }

            var fields = documentPropertiesWithAllAttributes(drcCardFields);


            foreach (var property in fields)
            {
                // Add the field, the property and method to the class.
                classDeclaration = classDeclaration.AddMembers(property);
            }

            return classDeclaration;
        }

        private IEnumerable<PropertyDeclarationSyntax> documentPropertiesWithAllAttributes(IEnumerable<Field> drcCardFields)
        {
            List<PropertyDeclarationSyntax> properties = new List<PropertyDeclarationSyntax>();


            foreach (var field in drcCardFields)
            {


                if (!field.AttributeName.Contains("."))
                {

                    if (field.Type == FieldType.ComplexTypeElement || field.Type == FieldType.DetailElement ||
                        field.Type == FieldType.DynamicField || field.Type == FieldType.Enum)
                    {
                        var fieldbase = _documentGenerator.generateDocumentDexmoPropertiesDeclarationWithAttributes(field);

                        properties.Add(fieldbase);

                    }
                    else if (field.Type == FieldType.RelationElement)
                    {

                        var mainProperty = _documentGenerator.generateDocumentRelationPropertyDeclarationWithAttributes(field);
                        //this is string key of relation element
                        properties.Add(mainProperty);

                        //there is a bridge table for relation documents
                        var documentFieldRelation = _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(field.Id);
                        if (documentFieldRelation != null)
                        {
                            var relatedDocumentName = _drcUnitOfWork.DrcCardRepository.getDrcCardName(documentFieldRelation.DrcCardId);
                            var camelCaseRelatedDocumentName = camelCaseDocumentName(relatedDocumentName);

                            var relationKey = _documentGenerator.generateDocumentRelationForeignKeyPropertyDeclaration(field, camelCaseRelatedDocumentName);

                            properties.Add(relationKey);
                        }

                    }
                    else
                    {
                        var fieldbase = _documentGenerator.generateDocumentPropertiesDeclarationWithAttributes(field);
                        properties.Add(fieldbase);

                    }
                }
            }

            _drcUnitOfWork.Complete();
            return properties;
        }


        // child interfaces of main document 
        private List<InterfaceDeclarationSyntax> generateChildInterfacesCodeOfMainDocument(List<Field> drcCardFields)
        {


            List<InterfaceDeclarationSyntax> innerInterfaces = new List<InterfaceDeclarationSyntax>();



            var fieldDictionary = new Dictionary<Field, List<Field>>();

            List<Field> inside = new List<Field>();
            foreach (var outfield in drcCardFields)
            {
                string[] words = outfield.AttributeName.Split(".");
                if (words.Length < 2 && (outfield.Type == FieldType.ComplexTypeElement || outfield.Type == FieldType.DetailElement || outfield.Type == FieldType.DynamicField))
                {
                    foreach (var field in drcCardFields)
                    {

                        string[] indsideString = field.AttributeName.Split(".");

                        if (indsideString.Length > 1)
                        {
                            string outField = _documentGenerator.cleanDetailElementName(words[0]);
                            string innerField = _documentGenerator.cleanDetailElementName(indsideString[0]);
                            if (innerField.Equals(outField, StringComparison.InvariantCultureIgnoreCase))
                            {
                                inside.Add(field);
                            }
                        }

                    }
                    fieldDictionary.Add(outfield, inside);
                    inside = new List<Field>();
                }

            }


            //until here my aim was grouping fields with one "."
            //now I am going to start creating documents

            foreach (var firstLayerInterface in fieldDictionary)
            {//this equals one of inner class interface

                var intercaseDeclarationSyntax = _documentGenerator.generateDetailComplexDynamicDocumentInterface(firstLayerInterface.Key.ItemName, firstLayerInterface.Key);


                var fields = new List<PropertyDeclarationSyntax>();
                if (firstLayerInterface.Value != null)
                {
                    //values and their layer
                    var cleanedfieldAtributeNamesFromDots = getFieldAttributeNamesWithoutDotsByTheirLayer(firstLayerInterface.Value);

                    //fields And their layers 
                    fields = documentPropertiesWithAllAttributes(cleanedfieldAtributeNamesFromDots).ToList();
                }


                foreach (var property in fields)
                {
                    // Add the field, the property and method to the class.
                    intercaseDeclarationSyntax = intercaseDeclarationSyntax.AddMembers(property);
                }

                innerInterfaces.Add(intercaseDeclarationSyntax);
            }


            return innerInterfaces;
        }

        //this method will find all enums in a document and will return list of enum class syntax
        private List<EnumDeclarationSyntax> generateEnums(List<Field> fields)
        {

            List<EnumDeclarationSyntax> enumDeclarationSyntaxes = new List<EnumDeclarationSyntax>();

            List<Field> enumFields = new List<Field>();

            foreach (var field in fields)
            {
                if (field.Type == FieldType.Enum)
                {
                    enumFields.Add(field);
                }
            }

            List<Field> uniqueEnumFields = getUniqueEnumFields(enumFields);


            foreach (var enumField in uniqueEnumFields)
            {
                var enumDeclaretion = _documentGenerator.generateEnumDeclaration(enumField);

                List<EnumMemberDeclarationSyntax> propertiesOfEnum = getPropertiesOfEnum(enumField.EnumValues);

                foreach (var enumProp in propertiesOfEnum)
                {
                    enumDeclaretion = enumDeclaretion.AddMembers(enumProp);
                }

                enumDeclarationSyntaxes.Add(enumDeclaretion);
            }

            return enumDeclarationSyntaxes;
        }

        private List<Field> getUniqueEnumFields(List<Field> enumFields)
        {
            Dictionary<string, List<Field>> reusedEnumsDictinary = new Dictionary<string, List<Field>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var enumField in enumFields)
            {
                if (!reusedEnumsDictinary.Any(a => a.Key.ToLower().Trim().Contains(enumField.ItemName.ToLower().Trim())))
                {
                    var sameFields = new List<Field>();
                    sameFields.Add(enumField);
                    reusedEnumsDictinary.Add(enumField.ItemName.ToLower().Trim(), sameFields);
                }
                else
                {
                    reusedEnumsDictinary[enumField.ItemName.Trim().ToLower()].Add(enumField);
                }
            }

            List<Field> uniqueFields = new List<Field>();
            foreach (var enumDictionary in reusedEnumsDictinary)
            {
                Field newEnumField = enumDictionary.Value.First();

                var uniqueProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var field in enumDictionary.Value)
                {
                    if (field.EnumValues != null)
                    {
                        List<String> enumProperties=field.EnumValues.Split(",").ToList();

                        foreach (var property in enumProperties)
                        {
                            if (!String.IsNullOrEmpty(property))
                            {
                                uniqueProperties.Add(property);
                            }
                          
                        }
                    }
                    
                }
                newEnumField.EnumValues = string.Join(",", uniqueProperties);
                uniqueFields.Add(newEnumField);
            }

            return uniqueFields;
        }

        private List<EnumMemberDeclarationSyntax> getPropertiesOfEnum(string enumValues)
        {


            List<EnumMemberDeclarationSyntax> enumMembers = new List<EnumMemberDeclarationSyntax>();

            if (enumValues != null)
            {
                string[] enumStrings = enumValues.Split(",");

                int value = 1;
                foreach (var enumMember in enumStrings)
                {
                    var member = _documentGenerator.generateEnumProperty(enumMember, value, enumStrings.Length);

                    value++;
                    enumMembers.Add(member);
                }
            }

            return enumMembers;
        }



        //fields and their interface layer
        private List<Field> getFieldAttributeNamesWithoutDotsByTheirLayer(List<Field> values)
        {
            List<Field> cleanFields = new List<Field>();

            foreach (var field in values)
            {
                string[] words = field.AttributeName.Split(".");
                field.AttributeName = words[1];
                cleanFields.Add(field);
            }


            return cleanFields;
        }



        private List<Field> cloneList(List<Field> fields)
        {
            List<Field> newlist = new List<Field>();
            foreach (var oldField in fields)
            {
                newlist.Add(_mapper.Map<Field>(oldField));

            }

            return newlist;
        }

        //In this method I am going to deep clone field list after that I will cut left side of each field atribute name to find inner complex types
        // by doing this I will process creating interfaces from top to inner ones
        //Lines[]                                                             -> complex
        //   Lines[].Id                                                          ->int                   1. seviye
        //   Lines[].ComplexA                                             -> complex
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //   ComplexA                                             ->complex
        //   ComplexA.Age                                       -> int                                      2. Seviye
        //    ComplexA.ComplexB                           -> complex
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    ComplexB                                      -> complex
        //    ComplexB.Value                                -> int                                           3. seviye
        private List<Field> cloneListByReshapingFieldAttribute(List<Field> fields, int layer)
        {
            List<Field> newlist = new List<Field>();
            foreach (var oldField in fields)
            {
                string newAttribute = AttributeReshaperForLayer(oldField, layer);
                if (newAttribute != null)
                {
                    oldField.AttributeName = newAttribute;
                    newlist.Add(oldField);
                }


            }

            return newlist;
        }
        public string AttributeReshaperForLayer(Field oldfield, int layer)
        {
            string newAttributeName = "";
            string[] fieldStrings = oldfield.AttributeName.Split(".");
            if (layer != 1)
            {
                if (fieldStrings.Length == layer &&
                    !(oldfield.Type == FieldType.ComplexTypeElement || oldfield.Type == FieldType.DetailElement || oldfield.Type == FieldType.DetailElement))
                {
                    return null;
                }
            }

            if (fieldStrings.Length < layer + 2)
            {
                string[] returnString = new string[2];
                int index = 0;
                for (int i = layer - 1; i < fieldStrings.Length; i++)
                {
                    returnString[index] = fieldStrings[i];
                    if (index < 1)
                    {
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (returnString[0] != null && returnString[1] != null)
                {
                    return returnString[0] + "." + returnString[1];
                }
                else if (returnString[0] != null && returnString[1] == null)
                {
                    return returnString[0];
                }
                else
                {
                    return null;
                }



            }
            else
            {
                return null;

            }

        }

        private string camelCaseDocumentName(string className)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            className = className.ToLower();
            className = myTI.ToTitleCase(className);
            className = className.Replace(" ", "").Trim();

            return className;
        }

        public string[] generateSubdomainVersionReportHtml(int subdomainId)
        {
            //example url
            //https://localhost:44347/DrcCards/presentation?subdomain=warehousemanagement&version=0.2.0

            string url;
            string subdomainName;
            SubdomainVersion subdomainVersion;
            string[] returnString = new string[2];
            try
            {
                var webAddress = _configuration.GetSection("WebSettings").GetSection("DrcDesignerWebAddress").Value;

                subdomainVersion = _drcUnitOfWork.SubdomainVersionRepository.GetById(subdomainId);
                subdomainName = _drcUnitOfWork.SubdomainRepository.GetSubdomainName(subdomainVersion.SubdomainId);
                subdomainName = subdomainName.ToLower().Replace(" ", "");

                url = webAddress + "DrcCards/presentation?subdomain=" + subdomainName + "&version=" + subdomainVersion.VersionNumber;




                returnString[0] = camelCaseDocumentName(subdomainName) + "_Version_" + subdomainVersion.VersionNumber;

                string htmlPage = _documentGenerator.generateHtmlPage(url);

                returnString[1] = htmlPage;
            }
            catch (Exception e)
            {
                // ignored
            }

            return returnString;
        }
    }
}
