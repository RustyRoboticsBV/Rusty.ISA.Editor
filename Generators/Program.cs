using Rusty.ActionGraph.Serialization;
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

// Generate base classes.
const string abstractFolder = target_folder + "Abstract/";
foreach (var @abstract in schema.Abstracts)
{
    string code = AbstractClassGenerator.Generate(schema, @abstract);
    string className = NameUtility.GetCodecName(@abstract.Name);
    Write(abstractFolder, className, code);
}

// Generate sealed classes.
const string elementFolder = target_folder + "Elements/";
foreach (var element in schema.Elements)
{
    string code = ElementClassGenerator.Generate(schema, element);
    string className = NameUtility.GetCodecName(element.Name);
    Write(elementFolder, className, code);
}

// Generate schema class.
Write(target_folder, "CodecSchema", SchemaClassGenerator.Generate(schema));

// Copy source files.
const string src_path = proj_path + "Src/";
Write(target_folder, "Codec", File.ReadAllText(src_path + "Codec.cs"));
Write(target_folder, "ICodecGroup", File.ReadAllText(src_path + "ICodecGroup.cs"));

// Generate binary serializer.
string binSerializerSkeleton = File.ReadAllText(src_path + "BinarySerializer.txt");

// 
Property prop = new()
{
    Type = "int",
    Name = "Cheese",
    Getter = new() { Implementation = new Code("5") }
};
Property prop2 = new()
{
    Type = "int",
    Name = "Cheese2",
    Getter = new() { Implementation = new Code("// e\nreturn 15;") }
};
Property prop3 = new()
{
    Type = "int",
    Name = "Cheese3",
    Getter = new() { Implementation = new Code("25") },
    Setter = new() { Implementation = new Code("e = 25") }
};
Property prop4 = new()
{
    Type = "int",
    Name = "Cheese4",
    Setter = new() { Implementation = new Code("e = 25") }
};
Property prop5 = new()
{
    Type = "int",
    Name = "Cheese5",
    Setter = new() { Implementation = new Code("//e\ne = 25;") }
};
Property prop6 = new()
{
    Access = AccessType.Internal,
    Modifier = ModifierType.Virtual,
    Type = "int",
    Name = "Cheese6",
    Getter = new() { Implementation = new Code("//e\nreturn 25;") },
    Setter = new() { Implementation = new Code("//e\ne = 25;") }
};
Console.WriteLine(prop);
Console.WriteLine(prop2);
Console.WriteLine(prop3);
Console.WriteLine(prop4);
Console.WriteLine(prop5);
Console.WriteLine(prop6);

// Write to file method.
static void Write(string folder, string name, string code)
{
    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    //var file = File.CreateText(folder + name + ".cs");
    //file.Write(code);
    //file.Close();
}