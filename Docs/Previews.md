# Previews
Instruction definitions, editor node infos, parameters and compile rules all have a preview field. These fields contain a string of GDScript that defines how the object's preview is computed in the editor. The editor generates preview classes for each object when they become relevant.

Each preview implementation should either be left empty, or return a string.

## Keywords
 Several special keywords are available when working with previews, which use the syntax `[[keyword]]`. The following are available:
- Instruction Definitions:
  - `[[name]]`, which gets replaced with the instruction's display name.
  - `[[parameter_id]]`, which gets replaced with a parameter's preview string.
  - `[[rule_id]]`, which gets replaced with a pre-instruction or post-instruction's preview string.
- Editor Node Info:
  - `[[instruction]]`, which gets replaced with the instruction definition's preview string.
  - `[[name]]`, which gets replaced with the instruction's display name.
  - `[[parameter_id]]`, which works the same as for instruction definitions.
  - `[[rule_id]]`, which works the same as for instruction definitions.
- Parameters:
  - `[[name]]`, which gets replaced with the parameter's display name.
  - `[[value]]`, which gets replaced with the parameter's value.
  - Output Parameters: `[[parameter_id]]`, which get replaced with another parameter's value. Only non-output parameters are allowed!
- Compile Rules:
  - `[[name]]`, which gets replaced with the rule's display name.
  - Instruction Rule:
    - `[[element]]`, which gets replaced with the instruction's preview string.
  - Option Rule:
    - `[[enabled]]`, which gets replaced with true if the option is enabled and false if it is disabled.
    - `[[element]]`, which gets replaced with the child rule's preview string if the option is enabled, and the empty string if it is disabled.
  - Choice Rule:
    - `[[selected]]`, which gets replaced with the index of the selected child rule.
    - `[[element]]`, which gets replaced with the selected child rule's preview string.
  - Tuple Rule:
    - `[[chile_rule_id]]`, which gets replaced with the specified child rule's preview string.
    - `[[element#]]`, which does the same as the above, but uses the tuple element index instead (for example, `[[element0]]` results in the first element's preview string).
    - `[[count]]`, which gets replaced with the number of elements in the tuple.
  - List Rule:
    - `[[element#]]`, which gets replaced with the preview string of the list element at the specified index (for example, `[[element0]]` results in the first element's preview string).
    - `[[count]]`, which gets replaced with the number of elements in the list.
    
The `[[element#]]` keywords of the tuple rule and list rule can also take a variable name. For example, you can do:
```
var result : String = "";
for i in [[count]]:
  result += [[elementi]];
return result;
```

## Helper methods
Several helper methods are available for usage in preview strings:
- `maxw(text : String, width : int) -> String`: limits the width of the string to the specified limit. For multi-line strings, this limit is applied to each line individually.
- `maxh(text : String, height : int) -> String`: limits the  of the string to the specified limit.
- `maxl(text : String, length : int) -> String`: limits the length of the string to the specified limit. Line-breaks are counted as single character here.
- `wwrap(text : String, width : int) -> String`: automatically breaks lines that are too wide. Lines are only broken on space and dash characters.

## Limitations
Several limitations exist to how previews may use the output of other previews, done in order to avoid the possibility of cyclic dependencies:
- Parameters cannot access the value of other parameters.
  - Except for outputs, which can only access non-output parameters.
- Compile rules can only access the preview strings of their direct children.
- Editor node infos may access the main instruction definition preview, but not the other way around.

## Notes
The editor node info's preview is used for the actual graph node. The instruction definition's preview, on the other hand, is used for the instruction inspector preview.