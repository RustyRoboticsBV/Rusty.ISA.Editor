# AGP File Format
The **AGP** (*ActionGraph Program*) file format is used to store the graphs created in the editor. It uses the extension `.agp` and is based on the XML format. Each file describes a single program graph, containing all of its elements and edges as well as metadata and schema (node and instruction definitions).

## General Rules
- The file root is always the `file` element.
- The inner text of elements is never used for values. Values are always stored in attributes.
- All `id` attributes must be unique among elements with the same tag within their scope.
- Elements reference schema definitions through `type` attributes containing the referenced definition's `id`.

---

# Schema Elements

## `file`

| Property | Value |
|---|---|
| Description | Root element. |
| Parent | *(none)* |
| Children | `meta`, `lang`, `idef`, `ndef`, `node`, `frame`, `joint`, `memo`, `edge` |
| Attributes | `csum` *(optional)* |

| Attribute | Description |
|---|---|
| `csum` | File checksum. |

---

## `meta`

| Property | Value |
|---|---|
| Description | Defines a metadata entry. |
| Parent | `file` |
| Children | *(none)* |
| Attributes | `id`, `value` |

| Attribute | Description |
|---|---|
| `id` | Metadata identifier. |
| `value` | Metadata value. |

---

## `lang`

| Property | Value |
|---|---|
| Description | Defines a language identifier used for parameter localization. |
| Parent | `file` |
| Children | *(none)* |
| Attributes | `id` |

| Attribute | Description |
|---|---|
| `id` | Identifier of the language. |

### Notes
- Languages are used for localizing parameters where `loc` equals `true`.
- The default values are the unlocalized argument values.

---

## `idef`

| Property | Value |
|---|---|
| Description | Defines an instruction. |
| Parent | `file` |
| Children | `pdef` |
| Attributes | `id`, `exec` |

| Attribute | Description |
|---|---|
| `id` | Instruction identifier / opcode. |
| `exec` | Execution handler identifier required by the target programming language. |

### Notes
- An `idef` may contain zero or more `pdef`s.

---

## `pdef`

| Property | Value |
|---|---|
| Description | Defines a parameter used by an instruction. |
| Parent | `idef` |
| Children | *(none)* |
| Attributes | `id`, `loc` *(optional)* |

| Attribute | Description |
|---|---|
| `id` | Parameter identifier. |
| `loc` | Boolean indicating whether this parameter supports localization. Defaults to `false`. |

### Notes
- A `pdef` only defines the existence of a parameter and whether it can be localized.

---

## `ndef`

| Property | Value |
|---|---|
| Description | Defines a node type. |
| Parent | `file` |
| Children | `odef`, `cdef`, `tdef`, `ldef`, `fdef` |
| Attributes | `id` |

| Attribute | Description |
|---|---|
| `id` | Node definition identifier. |

### Notes
- An `ndef` may have zero or more children.
- Child definitions may appear in any order.

---

## `odef`

| Property | Value |
|---|---|
| Description | Defines an option value. |
| Parent | `ndef`, `odef`, `cdef`, `tdef`, `ldef` |
| Children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |
| Attributes | `id` |

| Attribute | Description |
|---|---|
| `id` | Option definition identifier. |

### Notes
- An `odef` may have zero or one child definition.

---

## `cdef`

| Property | Value |
|---|---|
| Description | Defines a choice between multiple possible values. |
| Parent | `ndef`, `odef`, `cdef`, `tdef`, `ldef` |
| Children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |
| Attributes | `id` |

| Attribute | Description |
|---|---|
| `id` | Choice definition identifier. |

### Notes
- A `cdef` may have zero or more child definitions.
- Each child represents one possible choice.

---

## `tdef`

| Property | Value |
|---|---|
| Description | Defines a tuple value. |
| Parent | `ndef`, `odef`, `cdef`, `tdef`, `ldef` |
| Children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |
| Attributes | `id` |

| Attribute | Description |
|---|---|
| `id` | Tuple definition identifier. |

### Notes
- A `tdef` may have zero or more children.

---

## `ldef`

| Property | Value |
|---|---|
| Description | Defines a list value. |
| Parent | `ndef`, `odef`, `cdef`, `tdef`, `ldef` |
| Children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |
| Attributes | `id` |

| Attribute | Description |
|---|---|
| `id` | List definition identifier. |

### Notes
- An `ldef` may have zero or one children.
- Every element in the list uses the same child definition type.

---

## `fdef`

| Property | Value |
|---|---|
| Description | Defines a form, the editor representation of a single instruction. |
| Parent | `ndef`, `odef`, `cdef`, `tdef`, `ldef` |
| Children | `vadef`, `oadef` |
| Attributes | `id`, `type` |

| Attribute | Description |
|---|---|
| `id` | Form identifier. |
| `type` | Identifier of the instruction definition (`idef`) associated with this form. |

### Notes
- An `fdef` may contain zero or more arguments.
- Each `pdef` from the `idef` must be represented exactly once.

---

## `vadef`

| Property | Value |
|---|---|
| Description | A value argument definition. |
| Parent | `fdef` |
| Children | *(none)* |
| Attributes | `id`, `type` |

| Attribute | Description |
|---|---|
| `id` | Argument identifier. |
| `type` | Identifier of the referenced `pdef`. |

---

## `oadef`

| Property | Value |
|---|---|
| Description | An output argument definition. |
| Parent | `fdef` |
| Children | *(none)* |
| Attributes | `id`, `type`, `hidedf` |

| Attribute | Description |
|---|---|
| `id` | Output argument identifier. |
| `type` | Identifier of the referenced `pdef`. |
| `hidedf` | Hide the default output port if present? Defaults to `false`. |

### Notes
- A node's default output is hidden if it contains at least one `hidedf` that equals `true`.
---

# Graph Elements

## `node`

| Property | Value |
|---|---|
| Description | Defines a node instance in the graph. |
| Parent | `file` |
| Children | `form`, `option`, `choice`, `tuple`, `list` |
| Attributes | `id`, `type`, `x`, `y`, `member`, `start` |

| Attribute | Description |
|---|---|
| `id` | Node identifier. |
| `type` | Referenced `ndef` identifier. |
| `x` | X position. Defaults to `0`. |
| `y` | Y position. Defaults to `0`. |
| `member` | Containing frame identifier. Optional. |
| `start` | User-defined execution entry point name. Optional. |

### Notes
- Each child of the associated `ndef` must be represented exactly once.
- `start` names must be unique.

---

## `form`

| Property | Value |
|---|---|
| Description | Defines a form instance. |
| Parent | `node`, `option`, `choice`, `tuple`, `list` |
| Children | `arg`, `out` |
| Attributes | `type` |

| Attribute | Description |
|---|---|
| `type` | Referenced `fdef` identifier. |

### Notes
- Each child of the associated `fdef` must be represented exactly once.

---

## `arg`

| Property | Value |
|---|---|
| Description | A value argument. |
| Parent | `form` |
| Children | *(none)* |
| Attributes | `type`, `value` |

| Attribute | Description |
|---|---|
| `type` | Referenced `vadef` identifier. |
| `value` | Argument value string. |

---

## `out`

| Property | Value |
|---|---|
| Description | An output argument. |
| Parent | `form` |
| Children | *(none)* |
| Attributes | `type` |

| Attribute | Description |
|---|---|
| `type` | Referenced `oadef` identifier. |

---

## `option`

| Property | Value |
|---|---|
| Description | An option instance. |
| Parent | `node`, `option`, `choice`, `tuple`, `list` |
| Children | `form`, `option`, `choice`, `tuple`, `list` |
| Attributes | `type` |

| Attribute | Description |
|---|---|
| `type` | Referenced `odef` identifier. |

### Notes
- Contains zero or one child.
- The child must match the `odef`'s child.

---

## `choice`

| Property | Value |
|---|---|
| Description | Defines a selected choice instance. |
| Parent | `node`, `option`, `choice`, `tuple`, `list` |
| Children | `form`, `option`, `choice`, `tuple`, `list` |
| Attributes | `type` |

| Attribute | Description |
|---|---|
| `type` | Referenced `cdef` identifier. |

### Notes
- Contains zero or one child.
- The child selects one of the possible children of the referenced `cdef`.

---

## `tuple`

| Property | Value |
|---|---|
| Description | Defines a tuple instance. |
| Parent | `node`, `option`, `choice`, `tuple`, `list` |
| Children | `form`, `option`, `choice`, `tuple`, `list` |
| Attributes | `type` |

| Attribute | Description |
|---|---|
| `type` | Referenced `tdef` identifier. |

### Notes
- Contains exactly one child for each child definition of the referenced `tdef`.

---

## `list`

| Property | Value |
|---|---|
| Description | Defines a list instance. |
| Parent | `node`, `option`, `choice`, `tuple`, `list` |
| Children | `form`, `option`, `choice`, `tuple`, `list` |
| Attributes | `type` |

| Attribute | Description |
|---|---|
| `type` | Referenced `ldef` identifier. |

### Notes
- Contains zero or more children.
- All children must match the single child definition of the referenced `ldef`.

---

## `frame`

| Property | Value |
|---|---|
| Description | Defines an editor frame. |
| Parent | `file` |
| Children | *(none)* |
| Attributes | `id`, `x`, `y`, `width`, `height`, `member`, `text`, `color` |

| Attribute | Description |
|---|---|
| `id` | Frame identifier. |
| `x` | X position. Defaults to `0`. |
| `y` | Y position. Defaults to `0`. |
| `width` | Frame width. |
| `height` | Frame height. |
| `member` | Parent frame identifier. Optional. |
| `text` | Frame title text. |
| `color` | Frame color. |

---

## `joint`

| Property | Value |
|---|---|
| Description | Defines a graph joint. |
| Parent | `file` |
| Children | *(none)* |
| Attributes | `id`, `x`, `y`, `member` |

| Attribute | Description |
|---|---|
| `id` | Joint identifier. |
| `x` | X position. Defaults to `0`. |
| `y` | Y position. Defaults to `0`. |
| `member` | Containing frame identifier. Optional. |
| `edge` | Associated edge identifier. |

---

## `memo`

| Property | Value |
|---|---|
| Description | Defines a sticky note. |
| Parent | `file` |
| Children | *(none)* |
| Attributes | `id`, `x`, `y`, `member`, `text`, `color` |

| Attribute | Description |
|---|---|
| `id` | Memo identifier. |
| `x` | X position. Defaults to `0`. |
| `y` | Y position. Defaults to `0`. |
| `member` | Containing frame identifier. Optional. |
| `text` | Memo text. |
| `color` | Memo color. |

---

## `edge`

| Property | Value |
|---|---|
| Description | Defines a graph edge. |
| Parent | `file` |
| Children | *(none)* |
| Attributes | `id`, `from`, `port`, `to` |

| Attribute | Description |
|---|---|
| `id` | Edge identifier. |
| `from` | Source node identifier. |
| `port` | Source output port index. |
| `to` | Target node identifier. |
