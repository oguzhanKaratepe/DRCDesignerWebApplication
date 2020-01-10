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
using DRCDesigner.Business.BusinessModels;
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

            //I need also bring source document fields for shadow documents
            if (document.MainCardId != null)
            {
                var shadowSourceFields = _drcUnitOfWork.FieldRepository.getDrcCardAllFieldsWithoutTracking((int)document.MainCardId).ToList();

                foreach (var shadowField in shadowSourceFields)
                {
                    documentFields.Add(shadowField);
                }
            }

            var mainDocument = generateMainDocumentCode(document, documentFields);

            List<Field> allFieldsToGenerateComplexInterfaces=new List<Field>();

            int codeGenerationLevelSupport = 12;
            //codeGenerationLevelSupport means How many level code generation support do we want.
            //An Field name example:Lines[].DetailLines[].ComplexA.Count=> to generate code for this you need to dig  3 layer  
            //An Field name example:Lines[].DetailLines[].ComplexA.ComplexB.Details[].ID=> to generate code for this you need to dig 5 layer  
            for (int i = 1; i < codeGenerationLevelSupport; i++)
            {
                var firstFields = cloneList(documentFields);
                var fieldsForLayer = cloneListByReshapingFieldAttribute(firstFields, i);

                //this will stop digging fields deeper
                if (fieldsForLayer.Count == 0)
                {
                    break;
                }

                foreach (var field in fieldsForLayer)
                {
                    allFieldsToGenerateComplexInterfaces.Add(field);
                }
            }

            //Interfaces that for main document
            List<InterfaceDeclarationSyntax> childInterfacesOfMainDocument = generateChildInterfacesCodeOfMainDocument(allFieldsToGenerateComplexInterfaces);


            var fieldsForEnumGeneration = cloneList(documentFields);

            //I am planning to store enum first then I will compare that enums. Because same enum type could be use 
            List<EnumDeclarationSyntax> documentEnumsFromAllLayers = generateEnums(fieldsForEnumGeneration);




            // Add the interface classes to the namespace.
            @namespace = @namespace.AddMembers(mainDocument);

            foreach (var child in childInterfacesOfMainDocument)
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


            //in this fieldDictionary I have duclicates interfaces. 
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


            //in this fieldDictionary I have duclicates interfaces. So I need to merge these interfaces.
            var uniqueInterfaces = getUniqueInterfaces(fieldDictionary);

            //until here my aim was grouping fields with one "."
            //now I am going to start creating documents

            foreach (var firstLayerInterface in uniqueInterfaces)
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

        private Dictionary<Field, List<Field>> getUniqueInterfaces(Dictionary<Field, List<Field>> fieldDictionary)
        {
            Dictionary<Field, List<Field>> uniqueDictionary=new Dictionary<Field, List<Field>>();

            foreach (var interfacekeyValuePair in fieldDictionary)
            {
                
                if (!uniqueDictionary.Keys.Any(c => c.AttributeName.Trim().Equals(interfacekeyValuePair.Key.AttributeName.Trim(),StringComparison.InvariantCultureIgnoreCase)))
                {
                    uniqueDictionary.Add(interfacekeyValuePair.Key,interfacekeyValuePair.Value);
                }
                else
                {
                    //this means It is Dublicated Interface
                }
            
            }

            return uniqueDictionary;
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
                if (words.Length > 1)
                {
                    field.AttributeName = words[1];
                    cleanFields.Add(field);
                }
                else
                {
                    field.AttributeName = words[0];
                    cleanFields.Add(field);
                }
              
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


        public IEnumerable<DexmoVersionBusinessModel> getDexmoVersionOptions()
        {
            List<DexmoVersionBusinessModel> versions=new List<DexmoVersionBusinessModel>();

            List<string> versionStrings = _configuration.GetSection("DexmoVersions:Versions").Get<List<string>>();

            int i = 0;
            foreach (var versionName in versionStrings)
            {
                versions.Add(new DexmoVersionBusinessModel()
                {
                    Id = i,
                    DexmoVersion = versionName
                });
                i++;
            }

            return versions;
        }
    }
}
