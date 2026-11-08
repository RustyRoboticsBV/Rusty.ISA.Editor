#define RELEASE

using System;
using System.IO;

const string projPath = "../../../";

// Read JSON.
const string schemaPath = projPath + "schema.json";
string json = File.ReadAllText(schemaPath);
Schema schema = new(json);
Console.WriteLine(schema);

// Create directory.
#if RELEASE
const string targetFolder = projPath + "../Godot/Addons/ActionGraph/Serialization/";
#else
const string targetFolder = projPath + "Generated/";
#endif
if (Directory.Exists(targetFolder))
    Directory.Delete(targetFolder, true);
Directory.CreateDirectory(targetFolder);

const string elementsFolder = targetFolder + "Elements/";

// Generate base codec class.
BaseElementClassGenerator @base = new();
Write(elementsFolder, "Element", @base.Generate(schema));

// Generate base classes.
const string abstractFolder = elementsFolder + "Abstract/";
AbstractElementClassGenerator abstractGenerator = new();
foreach (var @abstract in schema.Abstracts)
{
    string code = abstractGenerator.Generate(schema, @abstract);
    string className = Names.ElementName(@abstract.Name);
    Write(abstractFolder, className, code);
}

// Generate sealed classes.
const string sealedFolder = elementsFolder + "Sealed/";
SealedElementClassGenerator sealedGenerator = new();
foreach (var element in schema.Sealeds)
{
    string code = sealedGenerator.Generate(schema, element);
    string className = Names.ElementName(element.Name);
    Write(sealedFolder, className, code);
}

// Generate interface class.
GroupInterfaceGenerator interfaceGenerator = new();
string interfaceCode = interfaceGenerator.Generate(schema);
Write(elementsFolder, "IElementGroup", interfaceCode);

// Generate schema class.
SchemaGenerator schemaGenerator = new();
string schemaCode = schemaGenerator.Generate(schema);
Write(targetFolder, "Elements", schemaCode);

// Generate hasher class.
HasherGenerator hasherGenerator = new();
string hasherCode = hasherGenerator.Generate(schema);
Write(targetFolder, "Hasher", hasherCode);

// Generate printer class.
ElementPrinterGenerator printerGenerator = new();
string printerCode = printerGenerator.Generate(schema);
Write(elementsFolder, "ElementPrinter", printerCode);

// Generate XML parser/serializer.
const string xmlFolder = targetFolder + "XML/";

XmlSerializerGenerator xmlSerializerGenerator = new();
string xmlSerializerCode = xmlSerializerGenerator.Generate(schema);
Write(xmlFolder, "XmlSerializer", xmlSerializerCode);

XmlParserGenerator xmlParserGenerator = new();
string xmlParserCode = xmlParserGenerator.Generate(schema);
Write(xmlFolder, "XmlParser", xmlParserCode);

// Generate binary parser/serializer.
const string binaryFolder = targetFolder + "Binary/";

BinarySerializerGenerator binarySerializerGenerator = new();
string binarySerializerCode = binarySerializerGenerator.Generate(schema);
Write(binaryFolder, "BinarySerializer", binarySerializerCode);

BinaryParserGenerator binaryParserGenerator = new();
string binaryParserCode = binaryParserGenerator.Generate(schema);
Write(binaryFolder, "BinaryParser", binaryParserCode);

// Documentation.
const string docsFolder = projPath + "../Docs/";
DomDocsGenerator domDocsGenerator = new();
string domDocs = domDocsGenerator.Generate(schema);
Write(docsFolder, "Schema", domDocs, "md");

// Write to file method.
static void Write(string folder, string name, string code, string extension = "cs")
{
    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    var file = File.CreateText(folder + name + "." + extension);
    file.Write(code);
    file.Close();
}