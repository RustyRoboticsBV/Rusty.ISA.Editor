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
|Allowed children|`metadata`, `language_definition`, `instruction_definition`, `node_definition`, `subgraph_definition`, `start_point`, `node`, `frame`, `memo`, `subgraph`, `edge`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this file.|
|`1` (`0x01`)|`editor`|`string`|The name of the editor that generated this file.|
|`2` (`0x02`)|`schema_version`|`string`|The ActionGraph schema version.|
|`3` (`0x03`)|`format_version`|`string`|The file format version.|
|`4` (`0x04`)|`default_language`|`string`|The ID of the default language when no localization is applied. This must NOT match any language definition IDs.|
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

### `option_definition`
|Property|Value|
|-|-|
|Index|`6` (`0x06`)|
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
|Index|`7` (`0x07`)|
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
|Index|`8` (`0x08`)|
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
|Index|`9` (`0x09`)|
|Description|A definition for a list inspector.|
|Allowed parents|`node_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Allowed children|`form_definition`, `option_definition`, `choice_definition`, `tuple_definition`, `list_definition`|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this list definition.|
---

### `form_definition`
|Property|Value|
|-|-|
|Index|`10` (`0x0A`)|
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

### `subgraph_definition`
|Property|Value|
|-|-|
|Index|`13` (`0x0D`)|
|Description|A definition for a subgraph, containing various graph elements.|
|Allowed parents|`file`|
|Allowed children|`node`, `frame`, `memo`, `subgraph`, `input`, `output`, `edge`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this subgraph definition.|
|`1` (`0x01`)|`text`|`string`|The title text of this subgraph definition.|
---

### `input`
|Property|Value|
|-|-|
|Index|`14` (`0x0E`)|
|Description|A subgraph instance entry point.|
|Allowed parents|`subgraph_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this input.|
|`1` (`0x01`)|`x`|`int`|The x-position of this input on the graph.|
|`2` (`0x02`)|`y`|`int`|The y-position of this input on the graph.|
|`3` (`0x03`)|`width`|`uint`|The width of this input on the graph.|
|`4` (`0x04`)|`height`|`uint`|The height of this input on the graph.|
|`5` (`0x05`)|`folded`|`bool`|Whether this input's inspector has been folded.|
|`6` (`0x06`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`7` (`0x07`)|`index`|`uint`|The index of this input on the subgraph's external connection layer.|
---

### `output`
|Property|Value|
|-|-|
|Index|`15` (`0x0F`)|
|Description|A subgraph instance exit point.|
|Allowed parents|`subgraph_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this output.|
|`1` (`0x01`)|`x`|`int`|The x-position of this output on the graph.|
|`2` (`0x02`)|`y`|`int`|The y-position of this output on the graph.|
|`3` (`0x03`)|`width`|`uint`|The width of this output on the graph.|
|`4` (`0x04`)|`height`|`uint`|The height of this output on the graph.|
|`5` (`0x05`)|`folded`|`bool`|Whether this output's inspector has been folded.|
|`6` (`0x06`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`7` (`0x07`)|`index`|`uint`|The index of this output on the subgraph's external connection layer.|
---

### `start_point`
|Property|Value|
|-|-|
|Index|`16` (`0x10`)|
|Description|A graph entry point, from which execution can begin.|
|Allowed parents|`file`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this start point.|
|`1` (`0x01`)|`x`|`int`|The x-position of this start point on the graph.|
|`2` (`0x02`)|`y`|`int`|The y-position of this start point on the graph.|
|`3` (`0x03`)|`width`|`uint`|The width of this start point on the graph.|
|`4` (`0x04`)|`height`|`uint`|The height of this start point on the graph.|
|`5` (`0x05`)|`folded`|`bool`|Whether this start point's inspector has been folded.|
---

### `node`
|Property|Value|
|-|-|
|Index|`17` (`0x11`)|
|Description|An editor node.|
|Allowed parents|`file`, `subgraph_definition`|
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
|`6` (`0x06`)|`folded`|`bool`|Whether this node's inspector has been folded.|
|`7` (`0x07`)|`parent_frame`|`string`|The identifier of the parent frame.|
---

### `memo`
|Property|Value|
|-|-|
|Index|`18` (`0x12`)|
|Description|An editor sticky note.|
|Allowed parents|`file`, `subgraph_definition`|
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
|`5` (`0x05`)|`folded`|`bool`|Whether this memo's inspector has been folded.|
|`6` (`0x06`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`7` (`0x07`)|`text`|`string`|The commentary text of this memo.|
|`8` (`0x08`)|`color`|`color`|The text color of this memo.|
---

### `frame`
|Property|Value|
|-|-|
|Index|`19` (`0x13`)|
|Description|A visual element grouping in the editor.|
|Allowed parents|`file`, `subgraph_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this frame.|
|`1` (`0x01`)|`x`|`int`|The x-position of this frame on the graph.|
|`2` (`0x02`)|`y`|`int`|The y-position of this frame on the graph.|
|`3` (`0x03`)|`width`|`uint`|The width of this frame on the graph.|
|`4` (`0x04`)|`height`|`uint`|The height of this frame on the graph.|
|`5` (`0x05`)|`folded`|`bool`|Whether this frame's inspector has been folded.|
|`6` (`0x06`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`7` (`0x07`)|`text`|`string`|The title text of this frame.|
|`8` (`0x08`)|`color`|`color`|The color of this frame.|
---

### `subgraph`
|Property|Value|
|-|-|
|Index|`20` (`0x14`)|
|Description|A subgraph instance.|
|Allowed parents|`file`, `subgraph_definition`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this subgraph.|
|`1` (`0x01`)|`definition`|`string`|The identifier of the associated subgraph definition.|
|`2` (`0x02`)|`x`|`int`|The x-position of this subgraph on the graph.|
|`3` (`0x03`)|`y`|`int`|The y-position of this subgraph on the graph.|
|`4` (`0x04`)|`width`|`uint`|The width of this subgraph on the graph.|
|`5` (`0x05`)|`height`|`uint`|The height of this subgraph on the graph.|
|`6` (`0x06`)|`folded`|`bool`|Whether this subgraph's inspector has been folded.|
|`7` (`0x07`)|`parent_frame`|`string`|The identifier of the parent frame.|
---

### `edge`
|Property|Value|
|-|-|
|Index|`21` (`0x15`)|
|Description|An editor graph edge.|
|Allowed parents|`file`, `subgraph_definition`|
|Allowed children|`joint`|
|Multiple children allowed?|true|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this edge.|
|`1` (`0x01`)|`from`|`string`|The identifier of the source element.|
|`2` (`0x02`)|`from_port`|`uint`|The port index on the source element.|
|`3` (`0x03`)|`to`|`string`|The identifier of the destination element.|
|`4` (`0x04`)|`to_port`|`uint`|The port index of the destination element.|
|`5` (`0x05`)|`thickness`|`uint`|The thickness of this edge.|
|`6` (`0x06`)|`color`|`color`|The color of this edge.|
---

### `joint`
|Property|Value|
|-|-|
|Index|`22` (`0x16`)|
|Description|A visual edge joint in the editor.|
|Allowed parents|`edge`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this joint.|
|`1` (`0x01`)|`x`|`int`|The x-position of this joint.|
|`2` (`0x02`)|`y`|`int`|The y-position of this joint.|
|`3` (`0x03`)|`size`|`uint`|The size of this joint.|
|`4` (`0x04`)|`parent_frame`|`string`|The identifier of the parent frame.|
|`5` (`0x05`)|`color`|`color`|The color of this joint.|
---

### `option`
|Property|Value|
|-|-|
|Index|`23` (`0x17`)|
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
---

### `choice`
|Property|Value|
|-|-|
|Index|`24` (`0x18`)|
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
---

### `tuple`
|Property|Value|
|-|-|
|Index|`25` (`0x19`)|
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
---

### `list`
|Property|Value|
|-|-|
|Index|`26` (`0x1A`)|
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
---

### `form`
|Property|Value|
|-|-|
|Index|`27` (`0x1B`)|
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
---

### `field`
|Property|Value|
|-|-|
|Index|`28` (`0x1C`)|
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
---

### `port`
|Property|Value|
|-|-|
|Index|`29` (`0x1D`)|
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
---

### `localized_value`
|Property|Value|
|-|-|
|Index|`30` (`0x1E`)|
|Description|A field's value when localized to a specific language.|
|Allowed parents|`metadata`, `field`|
|Allowed children|*(None)*|
|Multiple children allowed?|false|

**Attributes:**
|Index|Name|Type|Description|
|-|-|-|-|
|`0` (`0x00`)|`id`|`string`|The unique identifier of this localized value.|
|`1` (`0x01`)|`language`|`string`|The identifier of the associated language definition.|
|`2` (`0x02`)|`value`|`string`|The localized value.|
---