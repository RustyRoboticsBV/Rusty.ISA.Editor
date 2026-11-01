using System;
using System.IO;

const string proj_path = "../../../";

// Read JSON.
const string schema_path = proj_path + "schema.json";
string json = File.ReadAllText(schema_path);
Schema schema = new(json);
Console.WriteLine(schema);

// Create directory.
const string target_folder = proj_path + "Generated/";
if (Directory.Exists(target_folder))
    Directory.Delete(target_folder, true);
Directory.CreateDirectory(target_folder);

// Generate base codec class.
BaseElementClassGenerator @base = new();
Write(target_folder, "Element", @base.Generate(schema));

// Generate base classes.
const string abstractFolder = target_folder + "Abstract/";
AbstractElementClassGenerator abstractGenerator = new();
foreach (var @abstract in schema.Abstracts)
{
    string code = abstractGenerator.Generate(schema, @abstract);
    string className = Names.GetCodecName(@abstract.Name);
    Write(abstractFolder, className, code);
}

// Generate sealed classes.
const string elementFolder = target_folder + "Elements/";
SealedElementClassGenerator sealedGenerator = new();
foreach (var element in schema.Elements)
{
    string code = sealedGenerator.Generate(schema, element);
    string className = Names.GetCodecName(element.Name);
    Write(elementFolder, className, code);
}

// Generate interface class.
GroupInterfaceGenerator interfaceGenerator = new();
string interfaceCode = interfaceGenerator.Generate(schema);
Write(target_folder, "IElementGroup", interfaceCode);

// Write to file method.
static void Write(string folder, string name, string code)
{
    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    var file = File.CreateText(folder + name + ".cs");
    file.Write(code);
    file.Close();
}