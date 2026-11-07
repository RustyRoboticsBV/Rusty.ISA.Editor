# Schema

## General Rules
- Each file must have a `file` element as the root.
- All `id` attributes must be unique among elements with the same tag within their scope.

## Element Types

### `file`
|Property|Value|
|-|-|
|Index|`0` (`0x00`)|
|Description|The root element.|
|Allowed parents|*(None)*|
|Allowed children|`metadata`, `language_definition`, `instruction_definition`, `node_definition`, `node`, `frame`, `memo`, `joint`, `edge`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this file.|
|`1` (`0x01`)|`editor`|`string`|The name of the editor that generated this file.|
|`2` (`0x02`)|`schema_version`|`string`|The ActionGraph schema version.|
|`3` (`0x03`)|`format_version`|`string`|The file format version.|
|`4` (`0x04`)|`default_language`|`string`|The ID of the default language when no localization is applied. This must NOT match any language_definition IDs.|
|`5` (`0x05`)|`checksum`|`string`|The checksum of this file.|
---

### `language_definition`
|Property|Value|
|-|-|
|Index|`1` (`0x01`)|
|Description|A definition for a localization language.|
|Allowed parents|`file`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this language definition.|
---

### `metadata`
|Property|Value|
|-|-|
|Index|`2` (`0x02`)|
|Description|A metadata entry.|
|Allowed parents|`file`|
|Allowed children|`localized_value`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this metadata.|
|`1` (`0x01`)|`value`|`string`|The metadata value.|
---

### `instruction_definition`
|Property|Value|
|-|-|
|Index|`3` (`0x03`)|
|Description|A definition for a form inspector, which represents an instruction's state.|
|Allowed parents|`file`|
|Allowed children|`parameter_definition`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this instruction definition.|
|`1` (`0x01`)|`execution_handler`|`string`|The name of the class responsible for executing this instruction.|
---

### `parameter_definition`
|Property|Value|
|-|-|
|Index|`4` (`0x04`)|
|Description|A definition for an instruction parameter.|
|Allowed parents|`instruction_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this parameter definition.|
|`1` (`0x01`)|`localizable`|`bool`|Whether this parameter can be localized.|
---

### `node_definition`
|Property|Value|
|-|-|
|Index|`5` (`0x05`)|
|Description|A definition for a graph node.|
|Allowed parents|`file`|
|Allowed children|`form_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this node definition.|
---

### `form_definition`
|Property|Value|
|-|-|
|Index|`6` (`0x06`)|
|Description|A definition for an instruction inspector.|
|Allowed parents|`node_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Allowed children|`field_definition`, `port_definition`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this form definition.|
|`1` (`0x01`)|`instruction`|`string`|The identifier of the associated instruction definition.|
---

### `option_definition`
|Property|Value|
|-|-|
|Index|`7` (`0x07`)|
|Description|A definition for an option inspector.|
|Allowed parents|`node_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Allowed children|`form_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this option definition.|
---

### `choice_definition`
|Property|Value|
|-|-|
|Index|`8` (`0x08`)|
|Description|A definition for a choice inspector.|
|Allowed parents|`node_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Allowed children|`form_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this choice definition.|
---

### `tuple_definition`
|Property|Value|
|-|-|
|Index|`9` (`0x09`)|
|Description|A definition for a tuple inspector.|
|Allowed parents|`node_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Allowed children|`form_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this tuple definition.|
---

### `list_definition`
|Property|Value|
|-|-|
|Index|`10` (`0x0A`)|
|Description|A definition for a list inspector.|
|Allowed parents|`node_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Allowed children|`form_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this list definition.|
---

### `field_definition`
|Property|Value|
|-|-|
|Index|`11` (`0x0B`)|
|Description|A definition for a field inspector, which corresponds to a parameter.|
|Allowed parents|`form_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this field definition.|
|`1` (`0x01`)|`parameter`|`string`|The identifier of the associated parameter definition.|
---

### `port_definition`
|Property|Value|
|-|-|
|Index|`12` (`0x0C`)|
|Description|A definition for a node output port, which corresponds to a parameter.|
|Allowed parents|`form_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this port definition.|
|`1` (`0x01`)|`parameter`|`string`|The identifier of the associated parameter definition.|
|`2` (`0x02`)|`remove_default_port`|`bool`|Whether this port's presence removes a node's default port.|
---

### `node`
|Property|Value|
|-|-|
|Index|`13` (`0x0D`)|
|Description|An editor node.|
|Allowed parents|`file`|
|Allowed children|`form`, `option`, `choice`, `tuple`, `list`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this node.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated node definition.|
|`2` (`0x02`)|`x`|`int`|The x-position of this node on the graph.|
|`3` (`0x03`)|`y`|`int`|The y-position of this node on the graph.|
|`4` (`0x04`)|`width`|`uint`|The width of this node on the graph.|
|`5` (`0x05`)|`height`|`uint`|The height of this node on the graph.|
|`6` (`0x06`)|`folded`|`bool`|Whether this node has been folded.|
|`7` (`0x07`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`8` (`0x08`)|`text`|`string`|The annotation of this node.|
|`9` (`0x09`)|`color`|`color`|The color of this node.|
|`10` (`0x0A`)|`start_point_name`|`string`|The unique start point name, from which runtime execution of the graph can begin.|
---

### `memo`
|Property|Value|
|-|-|
|Index|`14` (`0x0E`)|
|Description|An editor sticky note.|
|Allowed parents|`file`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this memo.|
|`1` (`0x01`)|`x`|`int`|The x-position of this memo on the graph.|
|`2` (`0x02`)|`y`|`int`|The y-position of this memo on the graph.|
|`3` (`0x03`)|`width`|`uint`|The width of this memo on the graph.|
|`4` (`0x04`)|`height`|`uint`|The height of this memo on the graph.|
|`5` (`0x05`)|`folded`|`bool`|Whether this memo has been folded.|
|`6` (`0x06`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`7` (`0x07`)|`text`|`string`|The text of this memo.|
|`8` (`0x08`)|`color`|`color`|The text color of this memo.|
---

### `frame`
|Property|Value|
|-|-|
|Index|`15` (`0x0F`)|
|Description|A visual element grouping in the editor.|
|Allowed parents|`file`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this frame.|
|`1` (`0x01`)|`x`|`int`|The x-position of the frame on the graph.|
|`2` (`0x02`)|`y`|`int`|The y-position of this frame on the graph.|
|`3` (`0x03`)|`width`|`uint`|The width of this frame on the graph.|
|`4` (`0x04`)|`height`|`uint`|The height of this frame on the graph.|
|`5` (`0x05`)|`folded`|`bool`|Whether this frame has been folded.|
|`6` (`0x06`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`7` (`0x07`)|`text`|`string`|The title text of this frame.|
|`8` (`0x08`)|`color`|`color`|The color of this frame.|
---

### `joint`
|Property|Value|
|-|-|
|Index|`16` (`0x10`)|
|Description|A visual edge joint in the editor.|
|Allowed parents|`file`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this joint.|
|`1` (`0x01`)|`edge`|`string`|The identifier of the associated edge.|
|`2` (`0x02`)|`x`|`int`|The x-position of this joint.|
|`3` (`0x03`)|`y`|`int`|The y-position of this joint.|
|`4` (`0x04`)|`size`|`uint`|The size of this joint.|
|`5` (`0x05`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`6` (`0x06`)|`text`|`string`|The annotation of this joint.|
|`7` (`0x07`)|`color`|`color`|The color of this joint.|
---

### `edge`
|Property|Value|
|-|-|
|Index|`17` (`0x11`)|
|Description|An editor graph edge.|
|Allowed parents|`file`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this edge.|
|`1` (`0x01`)|`from`|`string`|The identifier of the source node.|
|`2` (`0x02`)|`port`|`uint`|The port index on the source node.|
|`3` (`0x03`)|`to`|`string`|The identifier of the destination node.|
|`4` (`0x04`)|`thickness`|`uint`|The thickness of this edge.|
|`5` (`0x05`)|`text`|`string`|The label of this edge.|
|`6` (`0x06`)|`color`|`color`|The color of this edge.|
---

### `form`
|Property|Value|
|-|-|
|Index|`18` (`0x12`)|
|Description|A form inspector, which represennts the state of an instruction.|
|Allowed parents|`node`, `option`, `choice`, `tuple`, `list`|
|Allowed children|`field`, `port`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this form.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated form definition.|
|`2` (`0x02`)|`folded`|`bool`|Whether this form has been folded.|
|`3` (`0x03`)|`text`|`string`|The annotation of this form.|
|`4` (`0x04`)|`color`|`color`|The color of this form.|
---

### `option`
|Property|Value|
|-|-|
|Index|`19` (`0x13`)|
|Description|An option inspector.|
|Allowed parents|`node`, `option`, `choice`, `tuple`, `list`|
|Allowed children|`form`, `option`, `choice`, `tuple`, `list`|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this option.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated option definition.|
|`2` (`0x02`)|`enabled`|`bool`|Whether this option has been enabled.|
|`3` (`0x03`)|`folded`|`bool`|Whether this option has been folded.|
|`4` (`0x04`)|`text`|`string`|The annotation of this option.|
|`5` (`0x05`)|`color`|`color`|The color of this option.|
---

### `choice`
|Property|Value|
|-|-|
|Index|`20` (`0x14`)|
|Description|A choice inspector.|
|Allowed parents|`node`, `option`, `choice`, `tuple`, `list`|
|Allowed children|`form`, `option`, `choice`, `tuple`, `list`|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this choice.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated choice definition.|
|`2` (`0x02`)|`selected`|`string`|The identifier of the selected item.|
|`3` (`0x03`)|`folded`|`bool`|Whether this choice has been folded.|
|`4` (`0x04`)|`text`|`string`|The annotation of this choice.|
|`5` (`0x05`)|`color`|`color`|The color of this choice.|
---

### `tuple`
|Property|Value|
|-|-|
|Index|`21` (`0x15`)|
|Description|A tuple inspector.|
|Allowed parents|`node`, `option`, `choice`, `tuple`, `list`|
|Allowed children|`form`, `option`, `choice`, `tuple`, `list`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this tuple.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated tuple definition.|
|`2` (`0x02`)|`folded`|`bool`|Whether this tuple has been folded.|
|`3` (`0x03`)|`text`|`string`|The annotation of this tuple.|
|`4` (`0x04`)|`color`|`color`|The color of this tuple.|
---

### `list`
|Property|Value|
|-|-|
|Index|`22` (`0x16`)|
|Description|A list inspector.|
|Allowed parents|`node`, `option`, `choice`, `tuple`, `list`|
|Allowed children|`form`, `option`, `choice`, `tuple`, `list`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this list.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated list definition.|
|`2` (`0x02`)|`folded`|`bool`|Whether this list has been folded.|
|`3` (`0x03`)|`text`|`string`|The annotation of this list.|
|`4` (`0x04`)|`color`|`color`|The color of this list.|
---

### `field`
|Property|Value|
|-|-|
|Index|`23` (`0x17`)|
|Description|A field inspector, which represents the state of an instruction argument.|
|Allowed parents|`form`|
|Allowed children|`localized_value`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this field.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated field definition.|
|`2` (`0x02`)|`value`|`string`|The non-localized value of this field.|
|`3` (`0x03`)|`text`|`string`|The annotation of this field.|
|`4` (`0x04`)|`color`|`color`|The color of this field.|
---

### `port`
|Property|Value|
|-|-|
|Index|`24` (`0x18`)|
|Description|A node output port, which represents the state of an instruction argument.|
|Allowed parents|`form`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this port.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated port definition.|
|`2` (`0x02`)|`index`|`uint`|The index of this port on its parent node.|
|`3` (`0x03`)|`text`|`string`|The annotation of this port port.|
|`4` (`0x04`)|`color`|`color`|The color of this port port.|
---

### `localized_value`
|Property|Value|
|-|-|
|Index|`25` (`0x19`)|
|Description|A field's value when localized to a specific language.|
|Allowed parents|`metadata`, `field`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this localized value.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the language definition.|
|`2` (`0x02`)|`value`|`string`|The localized value.|
---