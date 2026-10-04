# AGBP File Format
The **AGBP** (*ActionGraph Binary Program*) file format is used to store the graphs created in the editor. It uses the extension `.agbp`. Compared to the XML-based `.agxp` format, it is much more compact but non-readable. This can make it a more suitable format for large graphs. The format does NOT reduce the size of the compiled game, as Godot will import both formats into an identical resource. Each file describes a single program graph, containing all of its elements and edges as well as metadata and schema (node and instruction definitions).

Each of the elements from the `.axgp` format also exists in `.agbp`. The format always begins with a magic header: `0x00` `0x42` `0x49` `0x4E` `0x41` `0x47` `0x50` `0x00`, followed by the encoded `agbp` version string, followed by the encoded file element.
- Integers are encoded as ULEB128 varints.
- Strings are encoded as an encoded integer containing the string's bytesize, followed by the raw C# string's little-endian byte representation.
- Elements are encoded as:
  - A byte containing the element's hard-coded index.
  - If the element can have attributes:
    - A bitmask byte that stores which attributes are present, where the least-significant bit represents attribute 0. Which bit corresponds to which attribute is hard-coded for each element. 
    - The attribute values (as encoded strings) in the bitmask order.
  - If the element can have child elements: the number of children (ULEB128), followed by the encoded children.

|Index|Tag|Attribute0|Attribute1|Attribute2|Attribute3|Attribute4|Attribute5|Has children?|
|-|-|-|-|-|-|-|-|-|
|`00`|`file`|`editor`|`csum`|||||yes|
|`01`|`meta`|`name`|`value`|||||no|
|`02`|`lang`|`id`||||||no|
|`03`|`idef`|`id`|`exec`|||||yes|
|`04`|`pdef`|`id`|`loc`|||||no|
