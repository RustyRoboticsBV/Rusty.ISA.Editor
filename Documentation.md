# Compiler Documentation
This document describe the way in which the editor compiles structures of multiple instructions.

Elements within (parentheses) are optional, only being added when necessary.

The `EOG` instruction is used throughout the program to close groups of instructions.

### File Structure
```
PRO
├MTA
│ ├MD5
│ ├ISA
│ │ ├<definitions>
│ │ └EOG
│ └EOG
├GRA
│ ├<graph elements>
│ └EOG
└EOG
```

The `<definitions>` block may only contain `DEF` instructions. The `<graph elements>` block may only contain `NOD`, `JNT`, `CMT` and `FRM` instructions.

### Instruction Definition Metadata
```
DEF
├<parameters>
├(PRE)
│ ├<rules>
│ └EOG
├(PST)
│ ├<rules>
│ └EOG
└EOG
```

The `<parameters>` block may only contains `PAR` instructions. The `<rules>` blocks may only contains `RUL` and `REF` instructions.

### Compile Rule Metadata
```
RUL
├<child rules>
└EOG
```

The `<child rules>` block may only contain `RUL` and `REF` instructions.

### Graph Node
```
NOD
├(LAB)
├(BEG)
├(MBR)
├INS
│ ├(PRE)
│ │ ├<rules>
│ │ └EOG
│ ├<main instruction>
│ ├(PST)
│ │ ├<rules>
│ │ └EOG
│ └EOG
└EOG
```
The `rules` blocks may contain any number of `INS`, `OPT`, `CHO`, `TPL`, or `LST` rules.

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
