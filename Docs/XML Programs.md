# AGXP File Format
The **AGXP** (*ActionGraph XML Program*) file format is used to store the graphs created in the editor. It uses the extension `.agxp` and is based on the XML format. Each file describes a single program graph, containing all of its elements and edges as well as metadata and definitions.

## General Rules
- The element follows the rules outlined in the [schema](Schema.md).
- The inner text of elements is never used for values. Values are always stored in attributes.
- XML does not have types. Attribute values are therefore stored as strings:

|Type|Examples|
|-|-|
|bool|`"true"`, `"false"`|
|uint|`"123"`|
|int|`"100"`, `"-2"`|
|string|`"Hello, world!"`|
|color|`"#FF0000FF"`|

---
