# AGBP File Format
The **AGBP** (*ActionGraph Binary Program*) file format is used to store the graphs created in the editor. It uses the extension `.agbp`. Compared to the XML-based `.agxp` format, it is much more compact but non-readable. This can make it a more suitable format for large graphs. The format does NOT reduce the size of the compiled game, as Godot will import both formats into an identical resource. Each file describes a single program graph, containing all of its elements and edges as well as metadata and schema (node and instruction definitions).

The format defines three primitive data types:
- Raw byte: a single byte.
- Varint: a unsigned integer of any bytesize, in ULEB128 encoding.
- Text: a length-prefixed UTF-8 string. The length is encoded as a varint, and is followed by the UTF-8 value of that length.

The format always begins with the identifier bytes: `0x00` `0x42` `0x49` `0x4E` `0x41` `0x47` `0x50` `0x00`, followed by a text containing the `agbp` version, followed by the encoded file element.
- Elements are encoded as:
  - The element's hard-coded index (raw byte).
  - If the element can have attributes:
    - A bitmask of which attributes are present (raw byte), where the least-significant bit represents attribute 0. Which bit corresponds to which attribute is hard-coded for each element type. 
    - The attribute values (text) in the bit order.
  - If the element can have child elements: the number of children (varint), followed by the encoded children.

Each of the elements from the `.axgp` format also exists in `.agbp`. 

|Index|Tag|Attribute0|Attribute1|Attribute2|Attribute3|Attribute4|Attribute5|Has children?|
|-|-|-|-|-|-|-|-|-|
|`00`|`file`|`editor`|`csum`|||||yes|
|`01`|`meta`|`name`|`value`|||||no|
|`02`|`lang`|`id`||||||no|
|`03`|`idef`|`id`|`exec`|||||yes|
|`04`|`pdef`|`id`|`loc`|||||no|
