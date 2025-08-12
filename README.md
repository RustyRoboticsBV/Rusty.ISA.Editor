# Rusty.ISA.Editor
An editor app for the [Rusty.ISA](https://github.com/RustyRoboticsBV/Rusty.ISA) module.

It allows you to create *instruction set architecture* programs using a graph based interface, as well the instruction set itself. The `Process` class can execute these programs from within a Godot game.

## Program Graph Editor
This tab allows for the creation of ISA programs, using a graph-based interface.

[TODO: screenshot]

The editor has two components:
- The graph. It can be used to create, delete, select, move, connect and disconnect nodes. Multiple nodes can be selected at the same time.
- The inspector. It displays the contents of the selected nodes, and allows you to edit them. Each node comes with an optional start field, which allows you to define entry points from which the program can be executed.

[TODO: elaborate on usage]

The app saves programs in a CSV file format. You can save and load programs to and from a file or the clipboard.

## Adding Instructions
You can add instruction definitions to the editor by selecting the *Instruction Set* tab. Here, you can add, remove and edit instruction definitions. The instruction set files are stored in the `User` folder.

If you want the editor to be able to create nodes for an instruction, be sure to tick the *Has Editor Node* checkbox. If you leave this blank, then the instruction can only appear as a secondary instruction of other nodes.

Make sure to export your instruction set and add it to your Godot game project whenever you make any changes!
Also make sure to not create custom instructions that use opcodes that are already in use.

In addition to the [built-in instruction from the core module](https://github.com/RustyRoboticsBV/Rusty.ISA?tab=readme-ov-file#built-in-instructions), the following opcodes are reserved for *editor marker instructions*, used to preserve editability: `MD5`, `ISA`, `DEF`, `PAR`, `RUL`, `OPC`, `PRO`, `JNT` `CMT`, `FRM`, `MBR`, `NOD`, `INS`, `PRE`, `PST`, `OPT`, `CHO`, `TPL`, `LST`, `GTG`, `ENG`, `EOG`.<br/>These instructions ar editor-only, and are stripped from programs when imported into a game project.
