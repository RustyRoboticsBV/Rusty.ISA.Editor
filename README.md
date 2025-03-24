# Rusty.ISA.Editor
An editor app for the [Rusty.ISA](https://github.com/RustyRoboticsBV/Rusty.ISA) module.

It allows you to create *instruction set architecture* programs using a graph based interface, as well the instruction set itself. The `Process` class can execute these programs from within a Godot game.

## Program Graph Editor
This tab allows for the creation of ISA programs, using a graph-based interface.

[TODO: screenshot]

The editor has two components:
- The graph. It can be used to create, delete, select, move, connect and disconnect nodes. Multiple nodes can be selected at the same time.
- The inspector. It displays the contents of the selected nodes, and allows you to edit them. Each node comes with an optional start field, which allows you to define entry points from which the cutscene program can be played.

[TODO: elaborate on usage]

The app saves programs in a CSV file format. You can save and load programs to and from a file or the clipboard.

## Adding Instructions
You can add instruction definitions to the editor by selecting the *Instruction Set* tab. Here, you can add, remove and edit instruction definitions. The instruction set files are stored in the `User` folder.

If you want the editor to be able to create nodes for an instruction, be sure to tick the *Has Editor Node* checkbox. If you leave this blank, then the instruction can only appear as a secondary instruction of other nodes.

Make sure to export your instruction set and add it to your Godot game project whenever you make any changes!
Also make sure to not create custom instructions that use opcodes that are already in use.

## Built-in Instructions
In addition to the [built-in instruction from the core module](https://github.com/RustyRoboticsBV/Rusty.ISA?tab=readme-ov-file#built-in-instructions), the editor decorates generated programs with *marker instructions*. These markers serve to preserve editability, and group instructions together into editor structures so that a graph may be reconstructed. They have no in-game meaning. The markers include the following:
- `FRM(id, x, y, width, height, title, color)`: Starts a frame group.
  - `MBR(frame)`: designates a `FRM`, `CMT` or `NOD` group as being a member of some frame. 
- `CMT(x, y, text)`: Starts a comment group.
- `NOD(x, y)`: Starts a node group. It should always contain either a `GTO`, `END` or `INS` instruction. It may also contain an optional `BEG` instruction and/or an optional `LAB` instruction.
  - `INS()`: Starts an inspector group. It's meant for grouping a main instruction with its pre-instructions and post-instructions. The group should contain between one to three members: an optional `PRE` instruction, the main instruction and an optional `PST` instruction.
  - Secondary instruction markers:
	- `PRE()`: Starts a pre-instruction group.
	- `PST()`: Starts a post-instruction group.
  - Compile rule markers:
	- `OPT()`: Starts an option rule group. If the group has no members, then the
option was disabled.
	- `CHO(selected)`: Starts a choice rule group. The argument tells the editor that this instruction represents the nth selection in the dropdown window.
	- `TPL()`: Starts a tuple rule group.
	- `LST()`: Starts a list rule group.
- `CMT(x, y, text)`: Starts a comment group.
- `FRM(id, x, y, width, height, title, color_tint)`: Starts a frame group.
- `MBR(frame_id)`: If present in a `NOD`, `CMT` or `FRM` group, it designates that group as being a member of a frame.
- `MTA()`: Starts a metadata group. This group contains a truncated version of the instruction set, which is used to detect if instructions in the set have been changed since the last time that the cutscene was edited. If it is used, it should be the first instruction in the program, and there should be only one of them. The following members exist:
  - `DEF(opcode)`: Starts an instruction definition group. `PRE` and `PST` markers are used to define pre-instruction and post-instruction groups.
	- `PAR(type, id)`: A parameter definition.
	- `RUL(type, id)`: Starts a compile rule definition group. The type may be one of the following: `option`, `choice`, `tuple` or `list`.
	- `REF(opcode, id)`: Represents an instruction rule. 
- `EOG()`: Ends the most recently-started group.

By default, all editor-only instructions are stripped out when a program is loaded into Godot as a Program resource, so that no performance or filesize cost is incurred.
