# Compiler Documentation
This document describe the way in which the editor compiles structures of multiple instructions.
- Elements between (parentheses) are optional, only being added when necessary.
- Instructions separated by vertical bars ( | ) are alternatives, meaning only one of them can appear.
- Instructions followed by a Kleene star ( * ) indicate any number of repetitions of these instructions, including none.

The `EOG` instruction is used throughout the program to close groups of instructions.

### File Structure
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

### Compile Rule Metadata
```
RUL
├(RUL | REF)*
└EOG
```

### Graph Node
```
NOD
├(LAB)
├(BEG)
├(MBR)
├INS
│ ├(PRE)
│ │ ├(OPT | CHO | TPL | LST)*
│ │ └EOG
│ ├<main instruction>
│ ├(PST)
│ │ ├(OPT | CHO | TPL | LST)*
│ │ └EOG
│ └EOG
└EOG
```

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
