using System;
using System.IO;

const string path = "../../../schema.json";
string json = File.ReadAllText(path);
Schema schema = new(json);
Console.WriteLine(schema);

foreach (var name in schema.Abstracts)
{
    Console.WriteLine();
    Console.WriteLine(AbstractClassGenerator.Generate(schema, name));
}

foreach (var element in schema.Elements)
{
    Console.WriteLine();
    Console.WriteLine(ElementClassGenerator.Generate(schema, element));
}