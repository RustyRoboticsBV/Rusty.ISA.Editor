using System;
using System.IO;
using System.Xml.Linq;

// Read JSON.
const string path = "../../../schema.json";
string json = File.ReadAllText(path);
Schema schema = new(json);
Console.WriteLine(schema);

// Create directory.
const string target_folder = "../../../Generated";
if (Directory.Exists(target_folder))
    Directory.Delete(target_folder, true);
Directory.CreateDirectory(target_folder);

// Generate base classes.
const string abstractFolder = target_folder + "/Abstract";
foreach (var name in schema.Abstracts)
{
    string code = AbstractClassGenerator.Generate(schema, name);
    string className = NameUtility.GetCodecName(name);
    Write(abstractFolder, className, code);
}

// Generate sealed classes.
const string elementFolder = target_folder + "/Elements";
foreach (var element in schema.Elements)
{
    string code = ElementClassGenerator.Generate(schema, element);
    string className = NameUtility.GetCodecName(element.Name);
    Write(elementFolder, className, code);
}

// Generate schema class.
Write(target_folder, "CodecSchema", SchemaClassGenerator.Generate(schema));

// Write to file method.
static void Write(string folder, string name, string code)
{
    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    var file = File.CreateText(folder + "/" + name + ".cs");
    file.Write(code);
    file.Close();
}