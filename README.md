# Rusty.ISA.Editor
An editor app for the [ISA module](https://github.com/RustyRoboticsBV/Rusty.ISA).

It allows you to create:
- Programs of instructions, using an arbitrary instruction set.
- Create such instruction sets.

## Program Graph Editor
This tab allows for the creation of ISA programs, using a graph-based interface.

[TODO: screenshot]

The editor has three components:
- The graph. It can be used to create, delete, select, move, connect and disconnect nodes. Multiple nodes can be selected at the same time.
- The inspector. It displays the contents of the selected nodes, and allows you to edit them. Each node comes with an optional start field, which allows you to define entry points from which the program can be executed.
- The console. It displays messages, warnings and errors.

[TODO: elaborate on usage]

## Adding Instructions
You can add instruction definitions to be used in the editor by selecting the *Instruction Set* tab. Here you can create, delete and edit instruction definitions. The instruction set files are stored in the `User` folder.

If you want the editor to be able to create nodes for an instruction, be sure to tick the *Has Editor Node* checkbox. If you leave this blank, then the instruction can only appear as a secondary instruction of other nodes.

In addition to the [built-in instruction from the core module](https://github.com/RustyRoboticsBV/Rusty.ISA?tab=readme-ov-file#built-in-instructions), the following opcodes are reserved for *editor marker instructions*, used to preserve editability: `MTA`, `MD5`, `ISA`, `DEF`, `PAR`, `RUL`, `REF`, `PRO`, `JNT` `CMT`, `FRM`, `MBR`, `NOD`, `INS`, `PRE`, `PST`, `OPT`, `CHO`, `TPL`, `LST`, `GTG`, `ENG`, `EOG`.<br/>These instructions ar editor-only, and are stripped from programs when imported into a game project.
