# AGBP File Format
The **AGBP** (*ActionGraph Binary Program*) file format is used to store the graphs created in the editor. It uses the extension `.agbp`. Compared to the XML-based `.agxp` format, it is much more compact but non-readable. This can make it a more suitable format for large graphs. The format does NOT reduce the size of the compiled game, as Godot will import both formats into an identical resource. Each file describes a single program graph, containing all of its elements and edges as well as metadata and schema (node and instruction definitions).


### Data types

The format defines four primary data types:
- `bool`: a boolean value: `0x00` for `false` and `0x01` for `true`.
- `uint`: an unsigned integer of any bytesize, in ULEB128 encoding.
- `int`: a signed integer of any bytesize, in SLEB128 encoding.
- `string`: a length-prefixed UTF-8 string. The length is encoded as a `uint`, and is followed by the UTF-8 string of that length.
- `color`: a RGBA8 color value, encoded as RRGGBBAA.


### Document structure

The format always begins with:
- The format signature header: `41 47 42 50 00 00 00 00`.
- The format version `string`.
- The root element.


### Element structure

Elements are encoded as:
- A `uint` containing the element type index.
- The attribute values, using the order and types from the schema. All attributes must appear.
- If the element can have child elements:
  - A `uint` containing the number of children.
  - The encoded children.

View the [schema document](Schema.md) for more information about the different types of elements and their attributes.
