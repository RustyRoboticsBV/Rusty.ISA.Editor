# AGBP File Format
The **AGBP** (*ActionGraph Binary Program*) file format is used to store the graphs created in the editor. It uses the extension `.agbp`. Compared to the XML-based `.agxp` format, it is much more compact but non-readable. This can make it a more suitable format for large graphs. The format does NOT reduce the size of the compiled game, as Godot will import both formats into an identical resource. Each file describes a single program graph, containing all of its elements and edges as well as metadata and schema (node and instruction definitions).


### Data types

The format defines four primary data types:
- Raw byte: a single byte.
- Varint: a unsigned integer of any bytesize, in ULEB128 encoding.
- Text: a length-prefixed UTF-8 string. The length is encoded as a varint, and is followed by the UTF-8 string of that length.
- Element: the document contents. Each of the elements from the `.axgp` format has an equivalent in `.agbp` and vice-versa.

### Document structure

The format always begins with:
- The format identifier magic header (raw bytes): `00 00 41 47 42 50 00 00`.
- The format version (text).
- The encoded file element.


### Element structure

Elements are encoded as:
- The element's identifier (raw byte).
- If the element can have attributes:
  - A bitmask of which attributes are present (raw byte), where the least-significant bit represents attribute 0. Which bit corresponds to which attribute is hard-coded for each element type. 
  - The attribute values (text), in the order of the bitmask.
- If the element can have child elements:
  - The number of children (varint)
  - The encoded children (elements).


### List of elements

|Identifier|Tag|Attr0|Attr1|Attr2|Attr3|Attr4|Attr5|Attr6|Attr7|Has children?|
|-|-|-|-|-|-|-|-|-|-|-|
|`00`|`file`|`id`|`editor`|`csum`||||||&#x2705;|
|`01`|`meta`|`id`|`value`|||||||&#x274c;|
|`02`|`lang`|`id`||||||||&#x274c;|
|`03`|`idef`|`id`|`exec`|||||||&#x2705;|
|`04`|`pdef`|`id`|`loc`|||||||&#x274c;|
|`05`|`ndef`|`id`||||||||&#x2705;|
|`06`|`fdef`|`id`|`type`|||||||&#x2705;|
|`07`|`odef`|`id`|`type`|||||||&#x2705;|
|`08`|`cdef`|`id`|`type`|||||||&#x2705;|
|`09`|`tdef`|`id`|`type`|||||||&#x2705;|
|`0A`|`ldef`|`id`|`type`|||||||&#x2705;|
|`0B`|`vdef`|`id`|`type`|||||||&#x274c;|
|`0C`|`jdef`|`id`|`type`|`nodflt`||||||&#x274c;|
|`0D`|`node`|`id`|`type`|`x`|`y`|`member`|`start`|||&#x2705;|
|`0E`|`joint`|`id`|`x`|`y`|`member`|`edge`||||&#x274c;|
|`0F`|`frame`|`id`|`x`|`y`|`width`|`height`|`member`|`text`|`color`|&#x274c;|
|`10`|`memo`|`id`|`x`|`y`|`member`|`text`|`color`|||&#x274c;|
|`11`|`edge`|`id`|`from`|`port`|`to`|||||&#x274c;|
|`12`|`form`|`id`|`type`|||||||&#x2705;|
|`13`|`option`|`id`|`type`|||||||&#x2705;|
|`14`|`choice`|`id`|`type`|||||||&#x2705;|
|`15`|`tuple`|`id`|`type`|||||||&#x2705;|
|`16`|`list`|`id`|`type`|||||||&#x2705;|
|`17`|`arg`|`id`|`type`|`value`||||||&#x2705;|
|`18`|`loc`|`id`|`type`|`value`||||||&#x274c;|
|`19`|`out`|`id`|`type`|||||||&#x274c;|
