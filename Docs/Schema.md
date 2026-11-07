# Schema

## General Rules
- Each file must have a `file` element as the root.
- All `id` attributes must be unique among elements with the same tag within their scope.

## Element Types


### `file`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>0</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>The root element.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><i>(None)</i></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>metadata</code>, <code>language_definition</code>, <code>instruction_definition</code>, <code>node_definition</code>, <code>node</code>, <code>frame</code>, <code>memo</code>, <code>joint</code>, <code>edge</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this file.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>editor</code></td>
        <td><code>string</code></td>
        <td>The name of the editor that generated this file.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>schema_version</code></td>
        <td><code>string</code></td>
        <td>The ActionGraph schema version.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>format_version</code></td>
        <td><code>string</code></td>
        <td>The file format version.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>checksum</code></td>
        <td><code>string</code></td>
        <td>The checksum of this file.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `metadata`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>1</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A metadata entry.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this metadata.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>value</code></td>
        <td><code>string</code></td>
        <td>The metadata value.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `language_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>2</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a localization language.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this language definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `instruction_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>3</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a form inspector, which represents an instruction's state.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>parameter_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this instruction definition.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>execution_handler</code></td>
        <td><code>string</code></td>
        <td>The name of the class responsible for executing this instruction.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `parameter_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>4</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for an instruction parameter.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>instruction_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this parameter definition.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>localizable</code></td>
        <td><code>bool</code></td>
        <td>Whether this parameter can be localized.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `node_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>5</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a graph node.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this node definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `form_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>6</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for an instruction inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>field_definition</code>, <code>port_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this form definition.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>instruction</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated instruction definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `option_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>7</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for an option inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this option definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `choice_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>8</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a choice inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this choice definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `tuple_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>9</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a tuple inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this tuple definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `list_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>10</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a list inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form_definition</code>, <code>option_definition</code>, <code>choice_definition</code>, <code>tuple_definition</code>, <code>list_definition</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this list definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `field_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>11</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a field inspector, which corresponds to a parameter.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>form_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this field definition.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>parameter</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated parameter definition.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `port_definition`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>12</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A definition for a node output port, which corresponds to a parameter.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>form_definition</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this port definition.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>parameter</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated parameter definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>remove_default_port</code></td>
        <td><code>bool</code></td>
        <td>Whether this port's presence removes a node's default port.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `node`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>13</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>An editor node.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this node.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated node definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>x</code></td>
        <td><code>int</code></td>
        <td>The x-position of this node on the graph.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>y</code></td>
        <td><code>int</code></td>
        <td>The y-position of this node on the graph.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>width</code></td>
        <td><code>uint</code></td>
        <td>The width of this node on the graph.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>height</code></td>
        <td><code>uint</code></td>
        <td>The height of this node on the graph.</td>
      </tr>
      <tr>
        <td><code>6</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this node has been folded.</td>
      </tr>
      <tr>
        <td><code>7</code></td>
        <td><code>parent_frame</code></td>
        <td><code>string</code></td>
        <td>The identifier of the parent frame.</td>
      </tr>
      <tr>
        <td><code>8</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this node.</td>
      </tr>
      <tr>
        <td><code>9</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this node.</td>
      </tr>
      <tr>
        <td><code>10</code></td>
        <td><code>start_point_name</code></td>
        <td><code>string</code></td>
        <td>The unique start point name, from which runtime execution of the graph can begin.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `memo`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>14</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>An editor sticky note.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this memo.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>x</code></td>
        <td><code>int</code></td>
        <td>The x-position of this memo on the graph.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>y</code></td>
        <td><code>int</code></td>
        <td>The y-position of this memo on the graph.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>width</code></td>
        <td><code>uint</code></td>
        <td>The width of this memo on the graph.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>height</code></td>
        <td><code>uint</code></td>
        <td>The height of this memo on the graph.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this memo has been folded.</td>
      </tr>
      <tr>
        <td><code>6</code></td>
        <td><code>parent_frame</code></td>
        <td><code>string</code></td>
        <td>The identifier of the parent frame.</td>
      </tr>
      <tr>
        <td><code>7</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The text of this memo.</td>
      </tr>
      <tr>
        <td><code>8</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The text color of this memo.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `frame`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>15</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A visual element grouping in the editor.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this frame.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>x</code></td>
        <td><code>int</code></td>
        <td>The x-position of the frame on the graph.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>y</code></td>
        <td><code>int</code></td>
        <td>The y-position of this frame on the graph.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>width</code></td>
        <td><code>uint</code></td>
        <td>The width of this frame on the graph.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>height</code></td>
        <td><code>uint</code></td>
        <td>The height of this frame on the graph.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this frame has been folded.</td>
      </tr>
      <tr>
        <td><code>6</code></td>
        <td><code>parent_frame</code></td>
        <td><code>string</code></td>
        <td>The identifier of the parent frame.</td>
      </tr>
      <tr>
        <td><code>7</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The title text of this frame.</td>
      </tr>
      <tr>
        <td><code>8</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this frame.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `joint`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>16</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A visual edge joint in the editor.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this joint.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>edge</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated edge.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>x</code></td>
        <td><code>int</code></td>
        <td>The x-position of this joint.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>y</code></td>
        <td><code>int</code></td>
        <td>The y-position of this joint.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>size</code></td>
        <td><code>uint</code></td>
        <td>The size of this joint.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>parent_frame</code></td>
        <td><code>string</code></td>
        <td>The identifier of the parent frame.</td>
      </tr>
      <tr>
        <td><code>6</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this joint.</td>
      </tr>
      <tr>
        <td><code>7</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this joint.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `edge`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>17</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>An editor graph edge.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>file</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this edge.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>from</code></td>
        <td><code>string</code></td>
        <td>The identifier of the source node.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>port</code></td>
        <td><code>uint</code></td>
        <td>The port index on the source node.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>to</code></td>
        <td><code>string</code></td>
        <td>The identifier of the destination node.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>thickness</code></td>
        <td><code>uint</code></td>
        <td>The thickness of this edge.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The label of this edge.</td>
      </tr>
      <tr>
        <td><code>6</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this edge.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `form`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>18</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A form inspector, which represennts the state of an instruction.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>field</code>, <code>port</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this form.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated form definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this form has been folded.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this form.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this form.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `option`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>19</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>An option inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this option.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated option definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>enabled</code></td>
        <td><code>bool</code></td>
        <td>Whether this option has been enabled.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this option has been folded.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this option.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this option.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `choice`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>20</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A choice inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this choice.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated choice definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>selected</code></td>
        <td><code>string</code></td>
        <td>The identifier of the selected item.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this choice has been folded.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this choice.</td>
      </tr>
      <tr>
        <td><code>5</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this choice.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `tuple`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>21</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A tuple inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this tuple.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated tuple definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this tuple has been folded.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this tuple.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this tuple.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `list`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>22</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A list inspector.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>node</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>form</code>, <code>option</code>, <code>choice</code>, <code>tuple</code>, <code>list</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this list.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated list definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>folded</code></td>
        <td><code>bool</code></td>
        <td>Whether this list has been folded.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this list.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this list.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `field`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>23</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A field inspector, which represents the state of an instruction argument.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>form</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><code>localized_value</code></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>true</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this field.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated field definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>value</code></td>
        <td><code>string</code></td>
        <td>The non-localized value of this field.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this field.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this field.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `port`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>24</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A node output port, which represents the state of an instruction argument.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>form</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this port.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the associated port definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>index</code></td>
        <td><code>uint</code></td>
        <td>The index of this port on its parent node.</td>
      </tr>
      <tr>
        <td><code>3</code></td>
        <td><code>text</code></td>
        <td><code>string</code></td>
        <td>The annotation of this port port.</td>
      </tr>
      <tr>
        <td><code>4</code></td>
        <td><code>color</code></td>
        <td><code>color</code></td>
        <td>The color of this port port.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>

### `localized_value`
<table>
  <tr>
    <th>Property</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Index</td>
    <td><code>25</code></td>
  </tr>
  <tr>
    <td>Description</td>
    <td>A field's value when localized to a specific language.</td>
  </tr>
  <tr>
    <td>Parent</td>
    <td><code>field</code></td>
  </tr>
  <tr>
    <td>Children</td>
    <td><i>(None)</i></td>
  </tr>
    <td>Multiple children allowed?</td>
    <td>false</td>
  </tr>
  <tr>
    <td>Attributes</td>
    <td>
    <table>
      <tr>
        <th>Index</th>
        <th>Name</th>
        <th>Type</th>
        <th>Description</th>
      </tr>
      <tr>
        <td><code>0</code></td>
        <td><code>id</code></td>
        <td><code>string</code></td>
        <td>The unique identifier of this localized value.</td>
      </tr>
      <tr>
        <td><code>1</code></td>
        <td><code>definition</code></td>
        <td><code>string</code></td>
        <td>The identifier of the language definition.</td>
      </tr>
      <tr>
        <td><code>2</code></td>
        <td><code>value</code></td>
        <td><code>string</code></td>
        <td>The localized value.</td>
      </tr>
    </table>
    </td>
  </tr>
</table>
