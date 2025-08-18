# Compiler Documentation
This document describe the way in which the editor compiles structures of multiple instructions.
- Elements between (parentheses) are optional, only being added when necessary.
- Instructions separated by vertical bars ( | ) are alternatives, meaning only one of them can appear.
- Instructions followed by a Kleene star ( * ) indicate any number of repetitions of these instructions, including none.

Most instructions described here are editor scaffolding, or only having meaning at import time while being stripped from the runtime program.

The `EOG` instruction is used throughout the program to close groups of instructions.

## File Structure
The file structure follows the format below. A program must follow this format to be readable by the editor.

The `MD5` checksum instruction is not necessary for a program to be valid, but they are nevertheless always generated during compilation.

```
PRO
├MTA
│ ├MD5
│ ├ISA
│ │ ├(DEF)*
│ │ └EOG
│ └EOG
├GRA
│ ├(NOD | JNT | CMT | FRM)*
│ └EOG
└EOG
```

## Instruction Set Metadata
The `ISA` block contains a truncated copy of the instruction set. This allows the program importer to determine which instructions are editor-only (and can thus be stripped), as well as the number of parameters that each instruction should have.

In the future, this could also be used to detect changes to the instruction set when a program is opened in the editor.

### Instruction Definition Metadata
```
DEF
├(PAR)*
├(PRE)
│ ├(RUL | REF)*
│ └EOG
├(PST)
│ ├(RUL | REF)*
│ └EOG
└EOG
```

These blocks represent the metadata of a single instruction definition. Contained inside it is parameter, pre-instruction and post-instruction metadata. The header contains the opcode and editor-only flag.

### Compile Rule Metadata
```
RUL
├(RUL | REF)*
└EOG
```

These blocks describe the metadata of an option, choice, tuple or list rule. Instruction rule metadata is described using the `REF` instruction.

## Graph Elements
These structures are generated to maintain editability. Each represents a single element on the graph.

### Graph Node
```
NOD
├(LAB)
├(BEG)
├(MBR)
├INS
│ ├(PRE)
│ │ ├(INS | OPT | CHO | TPL | LST)*
│ │ └EOG
│ ├<main instruction>
│ ├(PST)
│ │ ├(INS | OPT | CHO | TPL | LST)*
│ │ └EOG
│ └EOG
└EOG
```

The `INS` blocks represent instruction inspectors. They contain a single "main" instruction, and their associated pre-instructions and post-instructions.

### Graph Comment
```
CMT
├(MBR)
└EOG
```

### Graph Joint
```
JNT
├(MBR)
└EOG
```

### Graph Frame
```
FRM
├(MBR)
└EOG
```

## Flow Control
These items are inserted by the compiler to handle flow control, preserving the graph structure at runtime.

### Goto Group
```
GTG
├(LAB)
├GTO
└EOG
```

Goto groups are inserted into edges if the following conditions are true:
- The edge connects to an element with multiple edges going into it.
- The edge is NOT the first to connect to said element.

## End Group
```
ENG
├(LAB)
├END
└EOG
```

End groups are inserted at empty output ports.
