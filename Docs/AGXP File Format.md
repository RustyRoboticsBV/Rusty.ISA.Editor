# AGXP File Format
The **AGXP** (*ActionGraph XML Program*) file format is used to store the graphs created in the editor. It uses the extension `.agxp` and is based on the XML format. Each file describes a single program graph, containing all of its elements and edges as well as metadata and schema (node and instruction definitions).

Only a few XML tags are allowed.
- `file`: The file root. Must be the top-level element. Only one may appear. May have a `csum` attribute, containing a checksum.
  - `meta`: A metadata entry. May only appear in the `file` block. Must have a unique `id` attribute (such as "name", "desc", "author" or "version") and a `value` attribute.
  - `lang`: A language definition. May only appear in the `file` block. Must have an unique `id` attribute.
  - `idef`: An instruction definition. May only appear in the `file` block. Must have a unique `id` attribute, and an `exec` attribute containing the execution handler's class name.
    - `pdef`: A parameter definition. May only appear in `idef` blocks. Must have a unique `id` attribute. May have a `loc` attribute, which must equal `true` if the parameter is localizable.
  - `ndef`: A node definition. May only appear in the `file` block. Must have a unique `id` attribute.
    - `odef`: An option definition. May only appear in `ndef` blocks. Must have a unique `id` attribute.
    - `cdef`: A choice definition. May only appear in `ndef` blocks. Must have a unique `id` attribute.
    - `tdef`: A tuple definition. May only appear in `ndef` blocks. Must have a unique `id` attribute.
    - `ldef`: A list definition. May only appear in `ndef` blocks. Must have a unique `id` attribute.
	- `fdef`: A form definition. May only appear in `ndef`, `odef`, `cdef`, `tdef` and `ldef` blocks. Must have a unique `id` attribute and a `type` attribute that must correspond to an `idef`'s `id`.
	  - `vadef`: A value argument definition. May only appear in `fdef` blocks. Must have an unique `id` attribute, and a `type` attribute that corresponds to an `pdef`'s `id`.
	  - `oadef`: An output argument definition. May only appear in `fdef` blocks. Must have an unique `id` attribute, and a `type` attribute that corresponds to an `pdef`'s `id`.
  - `node`: A node element. May only appear in the `graph` block. Must have a unique `id` attribute and a `type` attribute that must correspond to an `ndef`'s `id`. May only appear in the `elements` block.		- `form`: A form. May only appear in `node`, `option`, `choice`, `tuple` and `list` blocks. Must positionally correspond to an `fdef`.
	- `form`: A form. May only appear in `node`, `option`, `choice`, `tuple` and `list` blocks. Must positionally correspond to an `fdef`.
		- `arg`: An argument. May only appear in `form` blocks. Must positionally correspond to a `vadef` or an `oadef`. If it corresponds to an `oadef`, then its contents must match an `edge`'s `id`.
	- `option`: An option. May only appear in `node`, `option`, `choice`, `tuple` and `list` blocks. Must positionally correspond to an `odef`.
	- `choice`: A choice. May only appear in `node`, `option`, `choice`, `tuple` and `list` blocks. Must positionally correspond to a `cdef`.
	- `tuple`: A tuple. May only appear in `node`, `option`, `choice`, `tuple` and `list` blocks. Must positionally correspond to a `tdef`.
	- `list`: A list. May only appear in `node`, `option`, `choice`, `tuple` and `list` blocks. Must positionally correspond to an `ldef`.
  - `joint`: An edge joint element. May only appear in the `joints` block. Must have a unique `id` attribute.
  - `memo`: A sticky note element. May only appear in the `graph` block. Must have a unique `id` attribute.
  - `frame`: A frame element. May only appear in the `graph` block. Must have a unique `id` attribute.
  - `edge`: A graph edge. May only appear in the `edges` block. Must have a unique `id` attribute.
